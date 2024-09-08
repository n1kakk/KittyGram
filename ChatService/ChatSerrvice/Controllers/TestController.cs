using ClassLibrary1.Contracts;
using ClassLibrary1.InterfaceServices.IElasticSEarchService;
using ClassLibrary1.Models.PostgreModels.User;
using ClassLibrary1.Models.RedisUserActivity;
using ClassLibrary1.Publishers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using StackExchange.Redis;
using System.Data;

namespace ChatService.Controllers;

public class TestController: Controller
{
    private readonly IElasticSearchService _elasticsearchService;
    private readonly MessageInsertPublisher _messagePublisher;
    private readonly ILogger<TestController> _logger;

    private readonly IDbConnection _db;

    private readonly IDatabaseAsync _rdbAsync;
    public TestController(IElasticSearchService elasticsearchService, MessageInsertPublisher messagePublisher, ILogger<TestController> logger,
        IDbConnection db, ConnectionMultiplexer redis)
    {
        _elasticsearchService = elasticsearchService;
        _messagePublisher = messagePublisher;
        _logger = logger;


        _db = db;
        _rdbAsync = redis.GetDatabase();
    }

    [HttpGet("SetMessage")]
    [ProducesResponseType(200)]
    //public async Task<IActionResult> SetMessage(string nickName)
    //{
    //    var message1 = new MessageContract
    //    {
    //        MessageId = 1,
    //        ConversationId = 100,
    //        SenderNickname = "200",
    //        MessageType = 1,
    //        MessageContent = "Message 1 Content",
    //        Created = DateTime.Now,
    //        Deleted = 1,
    //        Updated = 1,
    //        Status = 0
    //    };

    //    var message2 = new MessageContract
    //    {
    //        MessageId = 2,
    //        ConversationId = 101,
    //        SenderNickname = "201",
    //        MessageType = 2,
    //        MessageContent = "Message 2 Content",
    //        Created = DateTime.Now,
    //        Deleted = 1,
    //        Updated = 1,
    //        Status = 1
    //    };
    //    var message3 = new MessageContract
    //    {
    //        MessageId = 3,
    //        ConversationId = 100,
    //        SenderNickname = "200",
    //        MessageType = 1,
    //        MessageContent = "Message 1 Content",
    //        Created = DateTime.Now,
    //        Deleted = 1,
    //        Updated = 1,
    //        Status = 0
    //    };

    //    var message4 = new MessageContract
    //    {
    //        MessageId = 4,
    //        ConversationId = 101,
    //        SenderNickname = "201",
    //        MessageType = 2,
    //        MessageContent = "Message 2 Content",
    //        Created = DateTime.Now,
    //        Deleted = 1,
    //        Updated = 1,
    //        Status = 1
    //    };
    //    var message5 = new MessageContract
    //    {
    //        MessageId = 5,
    //        ConversationId = 100,
    //        SenderNickname = "200",
    //        MessageType = 1,
    //        MessageContent = "Message 1 Content",
    //        Created = DateTime.Now,
    //        Deleted = 1,
    //        Updated = 1,
    //        Status = 0
    //    };

    //    var message6 = new MessageContract
    //    {
    //        MessageId = 6,
    //        ConversationId = 101,
    //        SenderNickname = "201",
    //        MessageType = 2,
    //        MessageContent = "Message 2 Content",
    //        Created = DateTime.Now,
    //        Deleted = 1,
    //        Updated = 1,
    //        Status = 1
    //    };
    //    var message7 = new MessageContract
    //    {
    //        MessageId = 7,
    //        ConversationId = 100,
    //        SenderNickname = "200",
    //        MessageType = 1,
    //        MessageContent = "Message 1 Content",
    //        Created = DateTime.Now,
    //        Deleted = 1,
    //        Updated = 1,
    //        Status = 0
    //    };

    //    var message8 = new MessageContract
    //    {
    //        MessageId = 8,
    //        ConversationId = 101,
    //        SenderNickname = "201",
    //        MessageType = 2,
    //        MessageContent = "Message 2 Content",
    //        Created = DateTime.Now,
    //        Deleted = 1,
    //        Updated = 1,
    //        Status = 1
    //    };
    //    var message9 = new MessageContract
    //    {
    //        MessageId = 9,
    //        ConversationId = 100,
    //        SenderNickname = "200",
    //        MessageType = 1,
    //        MessageContent = "Message 1 Content",
    //        Created = DateTime.Now,
    //        Deleted = 1,
    //        Updated = 1,
    //        Status = 0
    //    };

    //    var message10 = new MessageContract
    //    {
    //        MessageId = 10,
    //        ConversationId = 101,
    //        SenderNickname = "201",
    //        MessageType = 2,
    //        MessageContent = "Message 2 Content",
    //        Created = DateTime.Now,
    //        Deleted = 1,
    //        Updated = 1,
    //        Status = 1
    //    };
    //    var message11 = new MessageContract
    //    {
    //        MessageId = 11,
    //        ConversationId = 100,
    //        SenderNickname = "200",
    //        MessageType = 1,
    //        MessageContent = "Message 1 Content",
    //        Created = DateTime.Now,
    //        Deleted = 1,
    //        Updated = 1,
    //        Status = 0
    //    };

    //    var message12 = new MessageContract
    //    {
    //        MessageId = 12,
    //        ConversationId = 101,
    //        SenderNickname = "201",
    //        MessageType = 2,
    //        MessageContent = "Message 2 Content",
    //        Created = DateTime.Now,
    //        Deleted = 1,
    //        Updated = 1,
    //        Status = 1
    //    };
    //    bool result = await _messagePublisher.PublishMessageAsync(message1);
    //    if (result) _logger.LogInformation("Message1 published successfully.");

    //    bool result2 = await _messagePublisher.PublishMessageAsync(message2);
    //    if (result2) _logger.LogInformation("Message2 published successfully.");

    //    bool result3 = await _messagePublisher.PublishMessageAsync(message3);
    //    if (result3) _logger.LogInformation("Message2 published successfully.");

    //    bool result4 = await _messagePublisher.PublishMessageAsync(message4);
    //    if (result4) _logger.LogInformation("Message1 published successfully.");

    //    bool result5 = await _messagePublisher.PublishMessageAsync(message5);
    //    if (result5) _logger.LogInformation("Message2 published successfully.");

    //    bool result6 = await _messagePublisher.PublishMessageAsync(message6);
    //    if (result6) _logger.LogInformation("Message2 published successfully.");

    //    bool result7 = await _messagePublisher.PublishMessageAsync(message7);
    //    if (result7) _logger.LogInformation("Message1 published successfully.");

    //    bool result8 = await _messagePublisher.PublishMessageAsync(message8);
    //    if (result8) _logger.LogInformation("Message2 published successfully.");

    //    bool result9 = await _messagePublisher.PublishMessageAsync(message9);
    //    if (result9) _logger.LogInformation("Message2 published successfully.");

    //    bool result10 = await _messagePublisher.PublishMessageAsync(message10);
    //    if (result10) _logger.LogInformation("Message1 published successfully.");

    //    bool result11 = await _messagePublisher.PublishMessageAsync(message11);
    //    if (result11) _logger.LogInformation("Message2 published successfully.");

    //    bool result12 = await _messagePublisher.PublishMessageAsync(message12);
    //    if (result12) _logger.LogInformation("Message2 published successfully.");

    //    return Ok();
    //}


    [HttpGet("SetUserPostgres")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> SetUserPostgres(string NickName, DateTime lastactive)
    {
        string sqlQuery = "INSERT INTO \"User\" (nickname, lastactive)" +
            "VALUES (@NickName, @lastactive)";

        var parameters = new DynamicParameters();
        parameters.Add("@NickName", NickName);
        parameters.Add("@lastactive", lastactive);

        // Выполнение запроса и получение результата
        await _db.ExecuteAsync(sqlQuery, parameters);


        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok();
    }


    [HttpGet("SetFooRedis")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> SetFooRedis()
    {
        await _rdbAsync.StringSetAsync("foo", "bebebe");
        //Console.WriteLine(await _rdbAsync.StringGetAsync("foo"));
        var result = await _rdbAsync.StringGetAsync("foo");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(result.ToString());
    }


    [HttpGet("SetUserRedisHash")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> SetUserRedisHash(string nickname)
    {

        var userData = new RedisUserActivity
        {
            Nickname = nickname,
            LastActivity = DateTime.UtcNow,
           
        };

        await _rdbAsync.HashSetAsync($"user:{nickname}", new HashEntry[]
{
            new HashEntry(nameof(userData.Nickname), userData.Nickname),
            new HashEntry(nameof(userData.LastActivity), userData.LastActivity.ToString()),
                    });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok();
    }

    [HttpGet("SetConnectionRedis")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> SetConnectionRedis(string nickname, string connection)
    {
        await _rdbAsync.StringSetAsync(nickname, connection);
        //Console.WriteLine(await _rdbAsync.StringGetAsync("foo"));
        var result = await _rdbAsync.StringGetAsync(nickname);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(result.ToString());
    }

    [HttpGet("SetLastActivityRedis")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> SetLastActivityRedis(string nickname, string connection)
    {
        await _rdbAsync.StringSetAsync(nickname, connection);
        //Console.WriteLine(await _rdbAsync.StringGetAsync("foo"));
        var result = await _rdbAsync.StringGetAsync(nickname);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(result.ToString());
    }
}
