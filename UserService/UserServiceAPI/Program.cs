using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using NLog.Web;
using Npgsql;
using StackExchange.Redis;
using System;
using System.Data;
using UserServiceDAL.Helpers;
using UserServiceDAL.InterfaceRepository;
using UserServiceDAL.InterfaceServices;
using UserServiceDAL.Repositories;
using UserServiceDAL.Services;





var builder = WebApplication.CreateBuilder(args);
//builder.Configuration.AddEnvironmentVariables();
// Add services to the container.


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var connectionStringPostgres = builder.Configuration.GetConnectionString("PostgreSQLConnection");

builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(connectionStringPostgres));

//builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddSingleton(sp =>
{
    return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"));
});


var elasticsearchUri = builder.Configuration["Elasticsearch:Uri"];
builder.Services.AddSingleton<IElasticClient>(provider =>
{
    var settings = new ConnectionSettings(new Uri(elasticsearchUri));
    return new ElasticClient(settings);
});



builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection("SmtpConfig"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Logging.ClearProviders();
builder.Host.UseNLog();


builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBaseRepository, BaseRepository>();
builder.Services.AddScoped<IEmailVerificationRepository, EmailVerificationRepository>();
builder.Services.AddScoped<IElasticSearchRepository, ElasticSearchRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IElasticSearchService, ElasticSearchService>();

//builder.Services.AddScoped<ITokenService, JWTTokenService>();

//builder.Services.AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>();
//builder.Services.AddSingleton<IJWTTokenService, JWTTokenService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();


app.UseAuthorization();


app.MapControllers();



app.Run();

