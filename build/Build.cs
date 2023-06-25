using Nuke.Common;

internal class Build : NukeBuild, ICheckCodeFormatting, IRunTests
{
    //private Target Test => _ => _.Description("Running Tests").TryDependsOn<IRunTests>();

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => (x as IRunTests).RunTests);
}
