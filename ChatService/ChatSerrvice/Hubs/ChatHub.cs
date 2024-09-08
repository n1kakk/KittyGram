using AutoMapper;
using ClassLibrary1.Contracts;
using ClassLibrary1.InterfaceServices.IRedisService;
using ClassLibrary1.Models;
using ClassLibrary1.Models.PostgreModels.Message;
using ClassLibrary1.Models.RedisUserActivity;
using ClassLibrary1.Publishers;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;


namespace ChatService.Hubs;
public interface IChatClient
{
    //public Task ReceiveMessage(string nickName, string message, DateTime dateTime);
    public Task ReceiveMessage(MessageJS message);
}

public class ChatHub: Hub<IChatClient>
{
    private readonly IConnectionsRedisService _rdbConnService;
    private readonly ILastActiveRedisService _rdbActiveService;
    private readonly IDistributedCache _cache;
    private readonly MessageInsertPublisher _messagePublisher;
    private readonly IMapper _mapper;

    public ChatHub(IConnectionsRedisService rdbConnService, ILastActiveRedisService rdbActiveService, IDistributedCache cache,
        MessageInsertPublisher messagePublisher, IMapper mapper)
    {
        _rdbConnService = rdbConnService;
        _rdbActiveService = rdbActiveService;
        _cache = cache;
        _messagePublisher = messagePublisher;
        _mapper = mapper;
    }
    public async Task JoinChat(UserConnection userConnection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.ConversationName);

        var stringConnection = JsonSerializer.Serialize(userConnection);

        await _cache.SetStringAsync(Context.ConnectionId, stringConnection);
        //await Clients
        //.Group(userConnection.ConversationName).
        //.ReceiveMessage("Admin", $"{userConnection.Nickname} has joined", DateTime.Now);
 
        if (!string.IsNullOrEmpty(userConnection.Nickname))
        {
            await _rdbConnService.SetConnectionAsync(userConnection.Nickname, Context.ConnectionId);
            //await _rdbActiveService.DeleteUserActivityAsync(userConnection.Nickname);
            Console.WriteLine($"{userConnection.Nickname} is connected");
        }
        else
        {
            Console.WriteLine($"{userConnection.Nickname} banan....");
        }

    }
    public async Task LeaveRoom(UserConnection userConnection)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userConnection.ConversationName);
    }

    //public async Task SendMessage(string message)
    //{
    //    var stringConnection = await _cache.GetAsync(Context.ConnectionId);

    //    var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

    //    if (connection is not null)
    //    {
    //        await Clients
    //            .Group(connection.ConversationName)
    //            .ReceiveMessage(connection.Nickname, message);
    //    }
    //}
    public async Task SendMessage(MessageJS message)
    {
        var stringConnection = await _cache.GetAsync(Context.ConnectionId);

        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

        var messageToPublish = _mapper.Map<MessageContract>(message);
        await _messagePublisher.PublishMessageAsync(messageToPublish);
        //if (result5) _logger.LogInformation("Message2 published successfully.");

        if (connection is not null)
        {
            await Clients
                .Group(connection.ConversationName)
                //.ReceiveMessage(connection.Nickname, message.MessageContent, message.Created);
                .ReceiveMessage(message);
        }
            
    }
    //public override async Task OnConnectedAsync()
    //{
    //    var nickname = Context.GetHttpContext().Request.Query["nickname"];
    //    if (!string.IsNullOrEmpty(nickname))
    //    {
    //        await _rdbConnService.SetConnectionAsync(nickname, Context.ConnectionId);
    //        await _rdbActiveService.DeleteUserActivityAsync(nickname);
    //        Console.WriteLine($"{nickname} is connected");
    //    }
    //    else
    //    {
    //        Console.WriteLine($"{nickname} banan....");
    //    }

    //    await base.OnConnectedAsync();
    //}

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        //var nickname = Context.GetHttpContext().Request.Query["nickname"];
        //var userActivity = new RedisUserActivity
        //{
        //    Nickname = nickname,
        //    LastActivity = DateTime.UtcNow
        //};

        //await _rdbActiveService.SetUserActivityAsync(userActivity);
        //await _rdbConnService.DeleteConnectionAsync(Context.ConnectionId);


        var stringConnection = await _cache.GetAsync(Context.ConnectionId);
        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

        if (connection is not null)
        {   
            await _cache.RemoveAsync(Context.ConnectionId);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ConversationName);

            //await Clients
            //    .Group(connection.ConversationName)
            //    .ReceiveMessage("Admin", $"{connection.Nickname} left", DateTime.Now);
        }
        await base.OnDisconnectedAsync(exception);
    }

}
