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
                await DockerUtility.CreateDockerImage(PublishDirectory, GlobalSettings.RepositoryName, "latest");                
            });

        public Target UploadBuilds => _ => _
            .DependsOn(Package)
            .Description("Upload Builds to ECR")
            .Executes(async () =>
            {
                var assumedRole = await AwsIamUtilities.AssumeRole(GlobalSettings.ContainerRegistryAccessRole, "upload", null);
                await DockerUtility.PushDockerImageToEcr(GlobalSettings.AccountID,
                    GlobalSettings.ContainerRegistryAddress,
                    assumedRole.Credentials,
                    GlobalSettings.RepositoryName,
                    "latest");
            });



        public static int Main() => Execute<Build>(x => x.Compile);

        [Solution("LearnWebApi.sln")] readonly Solution Solution;

        public AbsolutePath SourceDirectory => RootDirectory / "src";
        public AbsolutePath OutputDirectory => RootDirectory / "output";
        public AbsolutePath PublishDirectory => RootDirectory / "publish";
    }
}
