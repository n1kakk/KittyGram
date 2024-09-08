using ChatService.Hubs;
using ClassLibrary1.Consumers.ElasticWorkerConsumer;
using ClassLibrary1.Consumers.PostgresWorkerConsumer;
using ClassLibrary1.Contracts;
using ClassLibrary1.Helpers;
using ClassLibrary1.InterfaceRepository.IElasticRepo;
using ClassLibrary1.InterfaceRepository.IPostgresRepo;
using ClassLibrary1.InterfaceRepository.IRedisRepo;
using ClassLibrary1.InterfaceServices.IElasticSEarchService;
using ClassLibrary1.InterfaceServices.IPostgresService;
using ClassLibrary1.InterfaceServices.IRedisService;
using ClassLibrary1.Publishers;
using ClassLibrary1.Repository.ElasticRepo;
using ClassLibrary1.Repository.PostgresRepo;
using ClassLibrary1.Repository.RedisRepo;
using ClassLibrary1.Services.ElasticSearchService;
using ClassLibrary1.Services.PostgresService;
using ClassLibrary1.Services.RedisService;
using MassTransit;
using Nest;
using Npgsql;
using ServiceStack;
using StackExchange.Redis;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy( policy =>
    {
        policy.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});


//REDIS CONNECTION
builder.Services.AddSingleton<ConnectionMultiplexer>(sp =>
{
    return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"));
});
 
builder.Services.AddStackExchangeRedisCache(options =>
{
    var connection = builder.Configuration.GetConnectionString("RedisConnection");
    options.Configuration = connection;
});

//POSTGRESQL CONNECTION
var connectionStringPostgres = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(connectionStringPostgres));


var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
var rabbitMqConfiguration = configuration.GetSection("MassTransit:RabbitMq");



builder.Services.AddMassTransit(x =>
{
    //Assembly workerConsumerPostgresAssembly = typeof(GettingStartedConsumer).Assembly;
    x.AddConsumer<WorkerConsumerPostgresInsert>();
    x.AddConsumer<WorkerConsumerElasticInsert>();

    x.AddConsumer<WorkerConsumerPostgresDelete>();
    x.AddConsumer<WorkerConsumerElasticDelete>();

    x.AddConsumer<WorkerConsumerPostgresEdit>();
    x.AddConsumer<WorkerConsumerElasticEdit>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqConfiguration["Host"], rabbitMqConfiguration["VirtualHost"], h =>
        {
            h.Username(rabbitMqConfiguration["UserName"]);
            h.Password(rabbitMqConfiguration["Password"]);
        });

        cfg.ReceiveEndpoint("messageInsertPostgres-queue", e =>
        {
            e.ConfigureConsumer<WorkerConsumerPostgresInsert>(context);
            e.Bind<MessageContract>(b =>
            {
                b.RoutingKey = "insertMessageKey";
            });
            
            e.ConfigureConsumeTopology = false;
        });

        cfg.ReceiveEndpoint("messageInsertElastic-queue", e =>
        {
            e.ConfigureConsumer<WorkerConsumerElasticInsert>(context);
            e.Bind<MessageContract>(b =>
            {
                b.RoutingKey = "insertMessageKey";
            });

            e.ConfigureConsumeTopology = false;
        });

        cfg.ReceiveEndpoint("messageDeletePostgres-queue", e =>
        {
            e.ConfigureConsumer<WorkerConsumerPostgresDelete>(context);
            e.Bind<DeleteMessageContract>(b =>
            {
                b.RoutingKey = "deleteMessageKey";
            });

            e.ConfigureConsumeTopology = false;
        });

        cfg.ReceiveEndpoint("messageDeleteElastic-queue", e =>
        {
            e.ConfigureConsumer<WorkerConsumerElasticDelete>(context);
            e.Bind<DeleteMessageContract>(b =>
            {
                b.RoutingKey = "deleteMessageKey";
            });

            e.ConfigureConsumeTopology = false;
        });

        cfg.ReceiveEndpoint("messageEditPostgres-queue", e =>
        {
            e.ConfigureConsumer<WorkerConsumerPostgresEdit>(context);
            e.Bind<UpdateMessageContract>(b =>
            {
                b.RoutingKey = "editMessageKey";
            });

            e.ConfigureConsumeTopology = false;
        });


        cfg.ReceiveEndpoint("messageEditElastic-queue", e =>
        {
            e.ConfigureConsumer<WorkerConsumerElasticEdit>(context);
            e.Bind<UpdateMessageContract>(b =>
            {
                b.RoutingKey = "editMessageKey";
            });

            e.ConfigureConsumeTopology = false;
        });



        cfg.Publish<MessageInsertPublisher>(p =>
        {
            p.ExchangeType = "direct";
        });


        cfg.Publish<MessageDeletePublisher>(p =>
        {
            p.ExchangeType = "direct";
        });

        cfg.Publish<MessageEditPublisher>(p =>
        {
            p.ExchangeType = "direct";
        });

    });
});
var elasticsearchUri = builder.Configuration["Elasticsearch:Uri"];
builder.Services.AddSingleton<IElasticClient>(provider =>
{
    var settings = new ConnectionSettings(new Uri(elasticsearchUri));
    return new ElasticClient(settings);
});

builder.Services.AddHostedService<WorkerConsumerElasticInsert>();
builder.Services.AddHostedService<WorkerConsumerElasticDelete>();
builder.Services.AddHostedService<WorkerConsumerElasticEdit>();

builder.Services.AddHostedService<WorkerConsumerPostgresInsert>();
builder.Services.AddHostedService<WorkerConsumerPostgresDelete>();
builder.Services.AddHostedService<WorkerConsumerPostgresEdit>();

builder.Services.AddSingleton<MessageInsertPublisher>();
builder.Services.AddSingleton<MessageDeletePublisher>();
builder.Services.AddSingleton<MessageEditPublisher>();


builder.Services.AddScoped<IElasticSearchRepository, ElasticSearchRepository>();
builder.Services.AddScoped<IElasticSearchService, ElasticSearchService>();


builder.Services.AddScoped<IConnectionsRedisService, ConnectionsRedisService>();
builder.Services.AddScoped<ILastActiveRedisService, LastActiveRedisService>();
builder.Services.AddScoped<IConnectionsRedisRepository, ConnectionsRedisRepository>();
builder.Services.AddScoped<ILastActiveRedisRepository, LastActiveRedisRepository>();


builder.Services.AddScoped<IBasePostgresRepository, BasePostgresRepository>();
builder.Services.AddScoped<IMessagePostgreRepository, MessagePostgreRepository>();
builder.Services.AddScoped<IMessagePostgresService, MessagePostgresService>();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.UseCors();
app.MapHub<ChatHub>("/chat");


app.Run();

