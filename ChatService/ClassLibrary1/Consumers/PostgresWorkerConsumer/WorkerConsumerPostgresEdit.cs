﻿using AutoMapper;
using ClassLibrary1.Contracts;
using ClassLibrary1.InterfaceServices.IPostgresService;
using ClassLibrary1.Models.PostgreModels.Message;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClassLibrary1.Consumers.PostgresWorkerConsumer;

public class WorkerConsumerPostgresEdit : BackgroundService, IConsumer<UpdateMessageContract>
{
    private static readonly List<UpdateMessageContract> _messageBuffer = new List<UpdateMessageContract>();

    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public WorkerConsumerPostgresEdit(IMapper mapper, IServiceScopeFactory serviceScopeFactory)
    {
        _mapper = mapper;
        _serviceScopeFactory = serviceScopeFactory;
    }
    public async Task Consume(ConsumeContext<UpdateMessageContract> context)
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
                List<UpdateDeleteMessage> messages = _mapper.Map<List<UpdateDeleteMessage>>(messageBatch);
                await EditMessagePostgresAsync(messages);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }

    private async Task EditMessagePostgresAsync(List<UpdateDeleteMessage> messages)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        var postgresMessageService = serviceScope.ServiceProvider.GetRequiredService<IMessagePostgresService>();
        await postgresMessageService.UpdateMessageAsync(messages);

    }
}
