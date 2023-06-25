using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

public interface ICleanSolution : INukeBuild
{
    [Solution]
    Solution Solution => TryGetValue(() => Solution);

    Target CleanSolution =>
        _ =>
            _.Description("Clean Solution")
                .Executes(() =>
                {
                    DotNetTasks.DotNetClean(x => x.SetProject(Solution));
                });
}
