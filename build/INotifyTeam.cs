using Nuke.Common;
using Serilog;

public interface INotifyTeam : INukeBuild
{
    [Parameter("Api Key")]
    string ApiKey => TryGetValue(() => ApiKey);

    [Parameter("Api Key")]
    [Secret]
    string Password => TryGetValue(() => Password);

    Target Notify =>
        _ =>
            _.Description("Notify Team")
                .Executes(() =>
                {
                    Log.Information(
                        "Pipeline finished with {ApiKey} and {Password}",
                        ApiKey,
                        Password
                    );
                });
}
