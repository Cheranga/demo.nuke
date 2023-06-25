using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

public interface IRestoreDotNetTools : INukeBuild
{
    Target RestoreDotNetTools =>
        _ =>
            _.Description("Restore DotNet Tools")
                .Executes(() =>
                {
                    DotNetTasks.DotNetToolRestore(x => x.SetProcessWorkingDirectory(RootDirectory));
                });
}
