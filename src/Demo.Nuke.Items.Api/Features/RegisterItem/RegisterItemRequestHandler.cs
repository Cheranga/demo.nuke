using Demo.Nuke.Items.Api.Services;

namespace Demo.Nuke.Items.Api.Features.RegisterItem;

public record RegisterItemRequest(string ItemId, string Sku, string category);

public record RegisterItemRequestHandler
{
    private readonly IMessagePublisher _messagePublisher;

    public RegisterItemRequestHandler(IMessagePublisher messagePublisher)
    {
        _messagePublisher = messagePublisher;
    }

    public async Task Register(RegisterItemRequest request)
    {
        // TODO: do better
        await _messagePublisher.PublishAsync(request);
    }
}
