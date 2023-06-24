using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.TypedResults;

namespace Demo.Nuke.Items.Api.Features.RegisterItem;

public static class RouterService
{
    public static void SetRegisterItemRoute(RouteGroupBuilder builder)
    {
        builder.MapPost(
            "/",
            async (
                [FromBody] RegisterItemRequest request,
                [FromServices] RegisterItemRequestHandler handler
            ) =>
            {
                await handler.Register(request);
                // TODO: publish an event to a queue
                return Ok();
            }
        );
    }
}
