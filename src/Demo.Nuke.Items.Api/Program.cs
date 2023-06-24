using Demo.Nuke.Items.Api.Features.RegisterItem;
using Demo.Nuke.Items.Api.Services;

const string route = "api/items";

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole().AddDebug();

builder.Services.AddSingleton<RegisterItemRequestHandler>();
builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();

var app = builder.Build();
var itemsApi = app.MapGroup(route);
RouterService.SetRegisterItemRoute(itemsApi);

app.Run();
