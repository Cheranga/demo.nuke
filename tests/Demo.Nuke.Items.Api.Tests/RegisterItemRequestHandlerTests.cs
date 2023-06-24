using Demo.Nuke.Items.Api.Features.RegisterItem;
using Demo.Nuke.Items.Api.Services;
using Moq;

namespace Demo.Nuke.Items.Api.Tests;

public class RegisterItemRequestHandlerTests
{
    [Fact]
    public async Task GivenValidRequestThenMustPublishRegisteredEvent()
    {
        var messagePublisher = new Mock<IMessagePublisher>();
        messagePublisher
            .Setup(x => x.PublishAsync(It.IsAny<RegisterItemRequest>()))
            .Returns(Task.FromResult(1));

        var handler = new RegisterItemRequestHandler(messagePublisher.Object);
        await handler.Register(new RegisterItemRequest("666", "item-666", "gardening"));
    }
}
