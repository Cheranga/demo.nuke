using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using DefaultNamespace;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Utilities.Text.Yaml;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Address Address { get; set; }
}

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}

internal class Build
    : NukeBuild,
        ICheckCodeFormatting,
        IRestoreDotNetTools,
        ICleanSolution,
        IRestoreSolution,
        ICompileSolution,
        IRunTests,
        INotifyTeam
{
    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode

    [Parameter]
    [Secret]
    private string ClientId;

    [Parameter]
    [Secret]
    private string ClientSecret;

    [Parameter]
    [Secret]
    private string TenantId;

    [Parameter]
    [Secret]
    private string SubscriptionId;

    Target ReadYml =>
        _ =>
            _.Description("Read Yml")
                .Executes(() =>
                {
                    //
                    // Read and verify YML
                    //
                    var ymlPath = RootDirectory / ".github" / "workflows" / "ci-pipeline.yml";
                    var person = ymlPath.ReadYaml<Person>();
                    var otherPerson = ymlPath.ReadYaml<dynamic>();

                    var someOtherPerson = new Person
                    {
                        Name = "Cheranga Hatangala",
                        Age = 40,
                        Address = new Address
                        {
                            City = "Melbourne",
                            Country = "Australia",
                            Street = "3000"
                        }
                    };
                    var otherYml = someOtherPerson.ToYaml();
                    var otherData = otherYml.GetYaml<dynamic>();

                    //
                    // Update and verify YML
                    //
                    ymlPath.UpdateYaml<Person>(x => x.Name = "Cheranga");
                    var updatedPerson = ymlPath.ReadYaml<Person>();

                    Assert.True(updatedPerson.Name == "Cheranga");
                });

    // private Target Init =>
    //     _ =>
    //         _.Description("Init")
    //             .Executes(() =>
    //             {
    //                 if (IsLocalBuild)
    //                 {
    //                     var host = Microsoft.Extensions.Hosting.Host
    //                         .CreateDefaultBuilder()
    //                         .ConfigureServices(
    //                             (context, services) =>
    //                             {
    //                                 var settings = context.Configuration
    //                                     .GetSection(nameof(AzureDeploySettings))
    //                                     .Get<AzureDeploySettings>();
    //
    //                                 services.AddSingleton(settings);
    //                             }
    //                         )
    //                         .ConfigureAppConfiguration(
    //                             (context, builder) =>
    //                             {
    //                                 builder.AddUserSecrets<AzureDeploySettings>(false, false);
    //                             }
    //                         )
    //                         .Build();
    //
    //                     var settings = host.Services.GetRequiredService<AzureDeploySettings>();
    //
    //                     (settings?.ClientId).NotNullOrEmpty("ClientId is not set");
    //
    //                     ClientId = settings.ClientId;
    //                     ClientSecret = settings.ClientSecret;
    //                     TenantId = settings.TenantId;
    //                     SubscriptionId = settings.SubscriptionId;
    //                 }
    //             });
    //
    // Target CreateResourceGroup =>
    //     _ =>
    //         _.Description("Create Resource Group")
    //             .DependsOn(Init)
    //             .Executes(async () =>
    //             {
    //                 var armClient = new ArmClient(
    //                     new ClientSecretCredential(TenantId, ClientId, ClientSecret),
    //                     SubscriptionId
    //                 );
    //                 var subscription = await armClient.GetDefaultSubscriptionAsync();
    //                 var rgName = "cc-test-blah-rg";
    //                 var location = AzureLocation.AustraliaSoutheast;
    //                 var operation = await subscription
    //                     .GetResourceGroups()
    //                     .CreateOrUpdateAsync(
    //                         WaitUntil.Completed,
    //                         rgName,
    //                         new ResourceGroupData(location)
    //                     );
    //                 var resourceGroup = operation.Value;
    //             });

    public static int Main() => Execute<Build>(x => (x as IRunTests).RunTests);
}
