using Demo.Nuke.Items.Api.Features.RegisterItem;

namespace Demo.Nuke.Items.Api.Services;

public interface IMessagePublisher
{
    Task PublishAsync(RegisterItemRequest request);
}

public record MessagePublisher : IMessagePublisher
{
    private readonly ILogger<MessagePublisher> _logger;

    public MessagePublisher(ILogger<MessagePublisher> logger)
    {
        _logger = logger;
    }

    public async Task PublishAsync(RegisterItemRequest request)
    {
        await Task
            .Delay(TimeSpan
                .FromSeconds(2));
        _logger.LogInformation("{@Request} published", request);
    }
}
