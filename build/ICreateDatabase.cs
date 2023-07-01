using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Serilog;
using Logger = Serilog.Core.Logger;

public interface ICreateDatabase : INukeComponent
{
    [Parameter("SQL Server Name")]
    string SqlServerName => TryGetValue(() => SqlServerName);

    [Parameter("Password")]
    [Secret]
    string SqlServerPassword => TryGetValue(() => SqlServerPassword);

    Target CreateDatabase =>
        _ =>
            _.Description("Create Database")
                .Requires(()=> SqlServerName, () => SqlServerPassword)
                .Executes(() =>
                {
                    Log.Information("{ServerName} and {Password}", SqlServerName, SqlServerPassword);
                    
                    DockerTasks.DockerRun(
                        settings =>
                            settings
                                .SetImage("mcr.microsoft.com/mssql/server:2019-latest")
                                .SetName(SqlServerName)
                                .SetProcessEnvironmentVariable("ACCEPT_EULA", "Y")
                                .SetProcessEnvironmentVariable("SA_PASSWORD", SqlServerPassword)
                                .SetPublish("1433:1433")
                                .EnableDetach()
                                .SetVolume("./Data:/var/opt/mssql")
                    );
                });
}
