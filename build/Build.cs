﻿using Amazon.CloudFormation.Model;
using Framework;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Settings;
using System.Collections.Generic;
using System.IO;

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
            .DependsOn(DeployDatabase)
            .DependsOn(DeployS3Bucket)
            .Description("Deploy the application to fargate")
            .Executes(async () => 
            {
                string applicationSecurityGroup = await AwsCloudformationUtils.GetStackOutputValue(EnvironmentSettings.ApplicationSecurityGroupStackName, "ApplicationSecurityGroup");
                string DbUsername = await AwsParameterStoreUtils.GetParameterValueAsync(EnvironmentSettings.DbUsernameParameterPath, false);
                string DbPassword = await AwsParameterStoreUtils.GetParameterValueAsync(EnvironmentSettings.DbPasswordParameterPath, false);
                string DBServerUrl = await AwsCloudformationUtils.GetStackOutputValue(EnvironmentSettings.DBServerStackName, "SQLDatabaseEndpoint");

                var parameters = new List<Parameter>
                {
                    new Parameter { ParameterKey = "CidrIp", ParameterValue = EnvironmentSettings.CidrIp },
                    new Parameter { ParameterKey = "Image", ParameterValue = $"{EnvironmentSettings.ContainerRegistryAddress}/{EnvironmentSettings.RepositoryName}:latest" },
                    new Parameter { ParameterKey = "SubnetIds", ParameterValue = EnvironmentSettings.SubnetIds },
                    new Parameter { ParameterKey = "SecurityGroups", ParameterValue = applicationSecurityGroup },
                    new Parameter { ParameterKey = "DBPassword", ParameterValue = DbPassword },
                    new Parameter { ParameterKey = "DBServerUrl", ParameterValue = DBServerUrl },
                    new Parameter { ParameterKey = "DBUsername", ParameterValue = DbUsername },
                    new Parameter { ParameterKey = "VpcId", ParameterValue = EnvironmentSettings.VpcId },
                };
                await AwsCloudformationUtils.CreateOrUpdateStack(EnvironmentSettings.ApplicationStackName,
                    TemplatesDirectory / "Application.yaml", parameters);
            });

        public Target DeployApplicationSecurityGroup => _ => _
            .Description("Deploy security groups for application")
            .Executes(async () =>
            {
                var parameters = new List<Parameter>
                {
                    new Parameter { ParameterKey = "CidrIp", ParameterValue = EnvironmentSettings.CidrIp },                    
                    new Parameter { ParameterKey = "VpcId", ParameterValue = EnvironmentSettings.VpcId },
                };
                await AwsCloudformationUtils.CreateOrUpdateStack(EnvironmentSettings.ApplicationSecurityGroupStackName,
                    TemplatesDirectory / "ApplicationSecurityGroups.yaml", parameters);
            });

        public Target DeployDatabase => _ => _
            .DependsOn(DeployApplicationSecurityGroup)
            .Description("Cloudformation of DB server")
            .Executes(async () =>
            {
                string DbUsername = await AwsParameterStoreUtils.GetParameterValueAsync(EnvironmentSettings.DbUsernameParameterPath, false);
                string DbPassword = await AwsParameterStoreUtils.GetParameterValueAsync(EnvironmentSettings.DbPasswordParameterPath, false);

                string applicationSecurityGroup = await AwsCloudformationUtils.GetStackOutputValue(EnvironmentSettings.ApplicationSecurityGroupStackName, "ApplicationSecurityGroup");
                var parameters = new List<Parameter>
                {                                        
                    new Parameter { ParameterKey = "ApplicationSecurityGroupId", ParameterValue = applicationSecurityGroup },
                    new Parameter { ParameterKey = "DBInstanceClass", ParameterValue = EnvironmentSettings.DBServerInstanceClass },
                    new Parameter { ParameterKey = "DBName", ParameterValue = EnvironmentSettings.PostgresDBName },
                    new Parameter { ParameterKey = "DBUsername", ParameterValue = DbUsername },
                    new Parameter { ParameterKey = "DBPassword", ParameterValue = DbPassword },
                    new Parameter { ParameterKey = "VpcId", ParameterValue = EnvironmentSettings.VpcId },
                };
                await AwsCloudformationUtils.CreateOrUpdateStack(EnvironmentSettings.DBServerStackName,
                    TemplatesDirectory / "PostgresDB.yaml", parameters);
            });

        public Target DeployS3Bucket => _ => _
            .Description("Cloudformation of S3 bucket")
            .Executes(async () =>
            {
                var parameters = new List<Parameter>
                {
                    new Parameter { ParameterKey = "SourceS3BucketName", ParameterValue = EnvironmentSettings.S3BucketName },
                    new Parameter { ParameterKey = "ArchiveS3BucketName", ParameterValue = EnvironmentSettings.S3ArchiveBucketName }
                };
                await AwsCloudformationUtils.CreateOrUpdateStack(EnvironmentSettings.S3BucketStackName,
                    TemplatesDirectory / "S3Bucket.yaml", parameters);
            });

        public static int Main() => Execute<Build>(x => x.Compile);

        [Solution("BookMaster.sln")] readonly Solution Solution;

        public AbsolutePath SourceDirectory => RootDirectory / "src";
        public AbsolutePath OutputDirectory => RootDirectory / "output";
        public AbsolutePath PublishDirectory => RootDirectory / "publish";
        public AbsolutePath TemplatesDirectory => RootDirectory / "build" / "Templates";
    }
}
