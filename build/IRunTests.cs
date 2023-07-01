using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using Serilog;

public interface IRunTests : INukeComponent
{
    public Target RunTests =>
        _ =>
            _.Description("Run Tests")
                .DependsOn<ICompileSolution>()
                .Triggers<INotifyTeam>()
                .Executes(() =>
                {
                    var envVariables =
                        EnvironmentInfo.Variables?.ToList()
                        ?? new List<KeyValuePair<string, string>>();

                    foreach (var keyValuePair in envVariables)
                    {
                        Log.Information("{Key} = {Value}", keyValuePair.Key, keyValuePair.Value);
                    }

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
