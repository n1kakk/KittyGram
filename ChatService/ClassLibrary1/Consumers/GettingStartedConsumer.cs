//using ClassLibrary1.Contracts;
//using MassTransit;
//using Microsoft.Extensions.Logging;


//namespace ClassLibrary1.Consumers
//{
//    public class GettingStartedConsumer: IConsumer<HelloMessage>
//    {
//        private readonly ILogger<GettingStartedConsumer> _logger;
//        public GettingStartedConsumer(ILogger<GettingStartedConsumer> logger)
//        {
//            _logger = logger;
//        }
//        public async Task Consume(ConsumeContext<HelloMessage> context)
//        {
//            await Task.Delay(TimeSpan.FromSeconds(5));
//            var message = context.Message;  
//            _logger.LogInformation($"Received message: {message}");
//            await Task.CompletedTask;
//        }
//    }
//}
