using Nuke.Common;
using Nuke.Common.ProjectModel;

public interface INukeComponent : INukeBuild
{
    [Solution]
    Solution Solution => TryGetValue(() => Solution);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    Configuration Configuration => TryGetValue(() => Configuration);
}
