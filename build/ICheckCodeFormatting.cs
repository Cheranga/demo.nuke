using Nuke.Common;
using Nuke.Common.Tools.DotNet;

public interface ICheckCodeFormatting : IRestoreDotNetTools
{
    Target CheckCodeFormatting =>
        _ =>
            _.Description("Check Code Formatting")
                .TryDependsOn<IRestoreDotNetTools>()
                .Executes(() =>
                {
                    DotNetTasks.DotNet($"csharpier --check .");
                });
}
