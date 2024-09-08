using ClassLibrary1.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ClassLibrary1.Publishers;

public class MessageInsertPublisher
{
    private readonly ILogger<MessageInsertPublisher> _logger;
    private readonly IBus _bus;
    public MessageInsertPublisher(ILogger<MessageInsertPublisher> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }
    public async Task<bool> PublishMessageAsync(MessageContract message)
    {
        await _bus.Publish(message, context =>
        {
            context.SetRoutingKey("insertMessageKey");
        });

        _logger.LogInformation("Published message");
        return true;
    }


}
