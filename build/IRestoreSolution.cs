using Nuke.Common;
using Nuke.Common.Tools.DotNet;

public interface IRestoreSolution : ICleanSolution
{
    Target RestoreSolution =>
        _ =>
            _.Description("Clean Solution")
                .DependsOn<ICleanSolution>()
                .Executes(() =>
                {
                    DotNetTasks.DotNetRestore(x => x.SetProjectFile(Solution));
                });
}
