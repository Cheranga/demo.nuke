using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Serilog;

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
                .Requires(() => SqlServerName, () => SqlServerPassword)
                .Executes(() =>
                {
                    Log.Information(
                        "{ServerName} and {Password}",
                        SqlServerName,
                        SqlServerPassword
                    );

                    DockerTasks.DockerRun(
                        settings =>
                            settings
                                .SetImage("mcr.microsoft.com/mssql/server:2019-latest")
                                .SetName(SqlServerName)
                                .SetEnv(
                                    "ACCEPT_EULA=Y",
                                    $"SA_PASSWORD={SqlServerPassword}",
                                    "MSSQL_PID=Standard"
                                )
                                .SetPublish("1433:1433")
                                .EnableDetach()
                                .EnableRm()
                                .SetVolume("./data:/var/opt/mssql")
                    );
                });
}