using Nuke.Common;
using Nuke.Common.Tools.DotNet;

public interface ICompileSolution : IRestoreSolution
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    Configuration Configuration => TryGetValue(() => Configuration);

    Target CompileSolution =>
        _ =>
            _.Description("Compile")
                .DependsOn<IRestoreSolution>()
                .Executes(() =>
                {
                    DotNetTasks.DotNetBuild(
                        x =>
                            x.SetProjectFile(Solution)
                                .EnableNoRestore()
                                .SetConfiguration(Configuration)
                    );
                });
}
