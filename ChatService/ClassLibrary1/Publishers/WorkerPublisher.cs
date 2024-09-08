//using ClassLibrary1.Contracts;
//using MassTransit;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;


//namespace ClassLibrary1.Publishers
//{
//    public class WorkerPublisher : BackgroundService
//    {
//        private readonly ILogger<WorkerPublisher> _logger;
//        private readonly IBus _bus;

//        public WorkerPublisher(IBus bus, ILogger<WorkerPublisher> logger)
//        {
//            _bus = bus;
//            _logger = logger;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            while (!stoppingToken.IsCancellationRequested)
//            {

//                await _bus.Publish(new HelloMessage
//                {
//                    Name = "Hello",
//                    Surname = "Bye"
//                });
//                _logger.LogInformation("Published");
//                await Task.Delay(1000, stoppingToken);
//            }
//        }
//    }
//}
