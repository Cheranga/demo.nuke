using System.Linq;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

[GitHubActions("build", GitHubActionsImage.UbuntuLatest, OnPushBranches = new[] { "master" })]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild
        ? Configuration.Debug
        : Configuration.Release;

    [Solution(GenerateProjects = true)]
    readonly Solution Solution;

    Target DotnetToolRestore =>
        _ =>
            _.Description("Restore DotNet Tools")
                .Executes(() =>
                {
                    DotNetTasks.DotNetToolRestore(x => x.SetProcessWorkingDirectory(RootDirectory));
                });

    Target CodeFormatCheck =>
        _ =>
            _.Description("Check C# Code Formatting")
                .DependsOn(DotnetToolRestore)
                .Executes(() =>
                {
                    DotNetTasks.DotNet($"csharpier --check .");
                });

    Target Clean =>
        _ =>
            _.Description("Clean Solution")
                .DependsOn(CodeFormatCheck)
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
                    DotNetTasks.DotNetBuild(
                        x =>
                            x.SetProjectFile(Solution)
                                .EnableNoRestore()
                                .SetConfiguration(Configuration)
                    );
                });

    Target Test =>
        _ =>
            _.Description("Run Tests")
                .DependsOn(Compile)
                .Executes(() =>
                {
                    Solution.AllProjects
                        .Where(x => x.Name.EndsWith("Tests"))
                        .ToList()
                        .ForEach(
                            x =>
                                DotNetTasks.DotNetTest(
                                    _ =>
                                        _.SetProjectFile(x)
                                            .EnableNoBuild()
                                            .EnableNoRestore()
                                            .SetConfiguration(Configuration)
                                )
                        );
                });

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Test);
}
