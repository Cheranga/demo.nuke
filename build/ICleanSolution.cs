using Nuke.Common;
using Nuke.Common.Tools.DotNet;

public interface ICleanSolution : INukeComponent
{
    Target CleanSolution =>
        _ =>
            _.Description("Clean Solution")
                .Executes(() =>
                {
                    DotNetTasks.DotNetClean(x => x.SetProject(Solution));
                });
}
