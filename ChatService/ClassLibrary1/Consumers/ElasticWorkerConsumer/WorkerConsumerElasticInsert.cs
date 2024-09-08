using ClassLibrary1.Contracts;
using ClassLibrary1.InterfaceServices.IElasticSEarchService;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace ClassLibrary1.Consumers.ElasticWorkerConsumer;

public class WorkerConsumerElasticInsert : BackgroundService, IConsumer<MessageContract>
{
    private readonly IServiceScopeFactory _serviseScopeFactory;
    private static readonly List<MessageContract> _messageBuffer = new List<MessageContract>();

    private readonly ILogger<WorkerConsumerElasticInsert> _logger;

    public WorkerConsumerElasticInsert(ILogger<WorkerConsumerElasticInsert> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviseScopeFactory = serviceScopeFactory;
    }

    public async Task Consume(ConsumeContext<MessageContract> context)
    {
        var message = context.Message;
        _logger.LogInformation($"Received message: {message}");

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

                await WriteToElasticAsync(messageBatch);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }

    private async Task WriteToElasticAsync(List<MessageContract> messageBatch)
    {
        using IServiceScope serviceScope = _serviseScopeFactory.CreateScope();
        var elasticSearchService = serviceScope.ServiceProvider.GetRequiredService<IElasticSearchService>();
        await elasticSearchService.SetMessagesAsync(messageBatch);

    }
}
