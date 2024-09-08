using AutoMapper;
using ClassLibrary1.Contracts;
using ClassLibrary1.InterfaceServices.IElasticSEarchService;
using ClassLibrary1.Models.PostgreModels.Message;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace ClassLibrary1.Consumers.ElasticWorkerConsumer;

public class WorkerConsumerElasticEdit : BackgroundService, IConsumer<UpdateMessageContract>
{
    private readonly IServiceScopeFactory _serviseScopeFactory;
    private static readonly List<UpdateMessageContract> _messageBuffer = new List<UpdateMessageContract>();
    private readonly IMapper _mapper;
    public WorkerConsumerElasticEdit(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
    {
        _serviseScopeFactory = serviceScopeFactory;
        _mapper = mapper;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_messageBuffer.Any())
            {
                var messageBatch = _messageBuffer.Take(10).ToList();
                _messageBuffer.RemoveRange(0, messageBatch.Count);

                await EditMessagesElasticAsync(messageBatch);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }

    public async Task Consume(ConsumeContext<UpdateMessageContract> context)
    {
        var message = context.Message;
        _messageBuffer.Add(message);
    }
    private async Task EditMessagesElasticAsync(List<UpdateMessageContract> messageBatch)
    {
        using IServiceScope serviceScope = _serviseScopeFactory.CreateScope();
        var elasticSearchService = serviceScope.ServiceProvider.GetRequiredService<IElasticSearchService>();
        List<UpdateDeleteMessage> messages = _mapper.Map<List<UpdateDeleteMessage>>(messageBatch);
        await elasticSearchService.EditMessagesAsync(messages);
    }
}
