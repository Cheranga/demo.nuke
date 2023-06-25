using System.Linq;
using Nuke.Common;
using Nuke.Common.Tools.DotNet;

public interface IRunTests : INukeComponent
{
    public Target RunTests =>
        _ =>
            _.Description("Run Tests")
                .DependsOn<ICompileSolution>()
                .Triggers<INotifyTeam>()
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
}
