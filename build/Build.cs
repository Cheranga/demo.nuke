using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild
        ? Configuration.Debug
        : Configuration.Release;

    [Solution]
    readonly Solution Solution;

    Target Clean =>
        _ =>
            _.Description("Clean Solution")
                //.Before(Restore)
                .Executes(() =>
                {
                    DotNetTasks.DotNetClean(x => x.SetProject(Solution));
                });

    Target Restore =>
        _ =>
            _.Description("Restore")
                .DependsOn(Clean)
                .Executes(() =>
                {
                    DotNetTasks.DotNetRestore(x => x.SetProjectFile(Solution));
                });

    Target Compile =>
        _ =>
            _.Description("Compile")
                .DependsOn(Restore)
                .Executes(() =>
                {
                    DotNetTasks.DotNetBuild(x => x.SetProjectFile(Solution));
                });
}
