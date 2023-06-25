using Nuke.Common;
using Nuke.Common.Tools.DotNet;

public interface ICompileSolution : INukeComponent
{
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
