using AutoMapper;
using ClassLibrary1.Contracts;
using ClassLibrary1.InterfaceServices.IPostgresService;
using ClassLibrary1.Models.PostgreModels.Message;
using ClassLibrary1.Publishers;
using MassTransit;
using MassTransit.Batching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace ClassLibrary1.Consumers.PostgresWorkerConsumer;

public class WorkerConsumerPostgresInsert : BackgroundService, IConsumer<MessageContract>
{
    private static readonly List<MessageContract> _messageBuffer = new List<MessageContract>();

    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory _serviseScopeFactory;

    private readonly MessageInsertPublisher _messageInsertPublisher;
    public WorkerConsumerPostgresInsert(IMapper mapper, IServiceScopeFactory serviseScopeFactory, MessageInsertPublisher messageInsertPublisher)
    {
        _mapper = mapper;
        _serviseScopeFactory = serviseScopeFactory;
        _messageInsertPublisher = messageInsertPublisher;
    }
    public async Task Consume(ConsumeContext<MessageContract> context)
    {
        var message = context.Message;
        //_logger.LogInformation($"Received message: {message}");

        _messageBuffer.Add(message);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_messageBuffer.Any())
            {
                var messageBatch = _messageBuffer.Take(10).ToList();
                _messageBuffer.RemoveRange(0, messageBatch.Count);
                List<Message> messages = _mapper.Map<List<Message>>(messageBatch);
                await WriteToPostgresAsync(messages);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }

    private async Task WriteToPostgresAsync(List<Message> messages)
    {
        using IServiceScope serviceScope = _serviseScopeFactory.CreateScope();
        var postgresMessageService = serviceScope.ServiceProvider.GetRequiredService<IMessagePostgresService>();
        await postgresMessageService.BulkInsertMessagesAsync(messages);
        var messageAckItems = messages.Select(message => new MessageInsertAckItem
        {
            MessageId = message.MessageId,
            TempId = message.TempId
        }).ToList();

        var messagesAckContract = new MessageInsertAckContract
        {
            InsertedMessages = messageAckItems
        };

    }
}
