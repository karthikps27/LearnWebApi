using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using System;
using System.Collections.Generic;
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
            .Executes(() =>
            {
                DotNetTasks.DotNetPublish(settings => settings
                .SetProject(Solution)
                .SetOutput(PublishDirectory));
            });

        public static int Main() => Execute<Build>(x => x.Compile);

        [Solution("LearnWebApi.sln")] readonly Solution Solution;

        public AbsolutePath SourceDirectory => RootDirectory / "src";
        public AbsolutePath OutputDirectory => RootDirectory / "output";
        public AbsolutePath PublishDirectory => RootDirectory / "publish";
    }
}
