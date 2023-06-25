using System.Linq;
using System.Reflection.Metadata;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Serilog;

// [GitHubActions("build", GitHubActionsImage.UbuntuLatest, OnPushBranches = new[] { "master" })]
internal class Build : NukeBuild
{
    [Parameter("Api key")]
    private readonly string ApiKey;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    private readonly Configuration Configuration = IsLocalBuild
        ? Configuration.Debug
        : Configuration.Release;

    [Solution(GenerateProjects = true)]
    private readonly Solution Solution;

    private Target Start =>
        _ =>
            _.Description("Start")
                .Executes(() =>
                {
                    Log.Information("{ApiKey} Starting pipeline", ApiKey);
                });

    private Target DotnetToolRestore =>
        _ =>
            _.Description("Restore DotNet Tools")
                .Executes(() =>
                {
                    DotNetTasks.DotNetToolRestore(x => x.SetProcessWorkingDirectory(RootDirectory));
                });

    private Target CodeFormatCheck =>
        _ =>
            _.Description("Check C# Code Formatting")
                .DependsOn(DotnetToolRestore)
                .Executes(() =>
                {
                    DotNetTasks.DotNet($"csharpier --check .");
                });

    private Target Clean =>
        _ =>
            _.Description("Clean Solution")
                .DependsOn(CodeFormatCheck)
                .Executes(() =>
                {
                    DotNetTasks.DotNetClean(x => x.SetProject(Solution));
                });

    private Target Restore =>
        _ =>
            _.Description("Restore")
                .DependsOn(Clean)
                .Executes(() =>
                {
                    DotNetTasks.DotNetRestore(x => x.SetProjectFile(Solution));
                });

    private Target Compile =>
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

    private Target Test =>
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
