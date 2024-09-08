using ClassLibrary1.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;


namespace ClassLibrary1.Publishers;

public class MessageEditPublisher
{
    private readonly ILogger<MessageEditPublisher> _logger;
    private readonly IBus _bus;
    public MessageEditPublisher(ILogger<MessageEditPublisher> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }
    public async Task<bool> PublishEditMessageAsync(UpdateMessageContract message)
    {
        await _bus.Publish(message, context =>
        {
            context.SetRoutingKey("editMessageKey");
        });

        _logger.LogInformation("Published edit message");
        return true;
    }
}
