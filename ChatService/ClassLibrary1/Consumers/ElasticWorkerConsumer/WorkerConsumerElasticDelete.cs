using AutoMapper;
using ClassLibrary1.Contracts;
using ClassLibrary1.InterfaceServices.IElasticSEarchService;
using ClassLibrary1.Models.PostgreModels.Message;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace ClassLibrary1.Consumers.ElasticWorkerConsumer;

public class WorkerConsumerElasticDelete : BackgroundService, IConsumer<DeleteMessageContract>
{
    private readonly IServiceScopeFactory _serviseScopeFactory;
    private static readonly List<DeleteMessageContract> _messageBuffer = new List<DeleteMessageContract>();
    private readonly IMapper _mapper;
    public WorkerConsumerElasticDelete(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
    {
        _serviseScopeFactory = serviceScopeFactory;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<DeleteMessageContract> context)
    {
        var message = context.Message;
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

                await DeleteFromElasticAsync(messageBatch);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }

    private async Task DeleteFromElasticAsync(List<DeleteMessageContract> messageBatch)
    {
        using IServiceScope serviceScope = _serviseScopeFactory.CreateScope();
        var elasticSearchService = serviceScope.ServiceProvider.GetRequiredService<IElasticSearchService>();
        List<UpdateDeleteMessage> messages = _mapper.Map<List<UpdateDeleteMessage>>(messageBatch);
        await elasticSearchService.DeleteMessagesAsync(messages);
    }
}
