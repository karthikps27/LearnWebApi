﻿using Amazon.CloudFormation.Model;
using Amazon.Runtime;
using Framework;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Build.Targets
{
    public class Build : NukeBuild
    {
        private EnvironmentSettings EnvironmentSettings => EnvironmentSettings.CreateSettingsInstance();
        public Target Clean => _ => _
            .Description("Cleaning previous build output")
            .Executes(() =>
            {
                FileSystemTasks.EnsureCleanDirectory(PublishDirectory);
            });

        public Target Compile => _ => _
            .DependsOn(Clean)
            .Description("Compile the project")
            .Executes(() =>
            {
                DotNetTasks.DotNetBuild(settings => settings.SetProjectFile(Solution));
            });

        public Target Package => _ => _
            .DependsOn(Compile)
            .Description("Publishing the build")
            .Executes(async () =>
            {
                DotNetTasks.DotNetPublish(settings => settings
                .SetProject(Solution)
                .SetOutput(PublishDirectory));

                FileSystemTasks.CopyFileToDirectory(Path.Combine(RootDirectory, "Dockerfile"), PublishDirectory);
                await DockerUtility.CreateDockerImage(PublishDirectory, EnvironmentSettings.RepositoryName, "latest");
            });

        public Target UploadBuilds => _ => _
            .DependsOn(Package)
            .Description("Upload Builds to ECR")
            .Executes(async () =>
            {
                EnvironmentSettings.CreateSettingsInstance();
                var assumedRole = await AwsIamUtilities.AssumeRole(EnvironmentSettings.ContainerRegistryAccessRole, "upload",
                    null);
                await DockerUtility.PushDockerImageToEcr(EnvironmentSettings.AccountID,
                    EnvironmentSettings.ContainerRegistryAddress,
                    assumedRole.Credentials,
                    EnvironmentSettings.RepositoryName,
                    "latest");
            });

        public Target DeployApplication => _ => _
            .DependsOn(UploadBuilds)
            .Description("Deploy the application to fargate")
            .Executes(async () => 
            {
                var parameters = new List<Parameter>
                {
                    new Parameter { ParameterKey = "CidrIp", ParameterValue = EnvironmentSettings.CidrIp },
                    new Parameter { ParameterKey = "Image", ParameterValue = $"{EnvironmentSettings.ContainerRegistryAddress}/{EnvironmentSettings.RepositoryName}:latest" },
                    new Parameter { ParameterKey = "SubnetIds", ParameterValue = EnvironmentSettings.SubnetIds },
                    new Parameter { ParameterKey = "VpcId", ParameterValue = EnvironmentSettings.VpcId },
                };
                await AwsCloudformationUtils.CreateOrUpdateStack($"{EnvironmentSettings.RepositoryName}-Application",
                    TemplatesDirectory / "Application.yaml", parameters);
            });

        public static int Main() => Execute<Build>(x => x.Compile);

        [Solution("LearnWebApi.sln")] readonly Solution Solution;

        public AbsolutePath SourceDirectory => RootDirectory / "src";
        public AbsolutePath OutputDirectory => RootDirectory / "output";
        public AbsolutePath PublishDirectory => RootDirectory / "publish";
        public AbsolutePath TemplatesDirectory => RootDirectory / "build" / "Templates";
    }
}
