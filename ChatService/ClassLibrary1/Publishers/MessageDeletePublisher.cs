using ClassLibrary1.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ClassLibrary1.Publishers;

public class MessageDeletePublisher
{
    private readonly ILogger<MessageDeletePublisher> _logger;
    private readonly IBus _bus;
    public MessageDeletePublisher(ILogger<MessageDeletePublisher> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }
    public async Task<bool> PublishDeleteMessageAsync(DeleteMessageContract message)
    {
        await _bus.Publish(message, context =>
        {
            context.SetRoutingKey("deleteMessageKey");
        });

        _logger.LogInformation("Published delete message");
        return true;
    }


}
