using ClassLibrary1.InterfaceRepository.IPostgresRepo;
using ClassLibrary1.Models.PostgreModels.Message;
using System.Data;
using Dapper;
using static MassTransit.Monitoring.Performance.BuiltInCounters;


namespace ClassLibrary1.Repository.PostgresRepo;
public class MessagePostgreRepository : IMessagePostgreRepository
{
    private readonly IDbConnection _db;
    public MessagePostgreRepository(IDbConnection db)
    {
        _db = db;   
    }


    public async Task<int> DeleteMessageForEveryoneAsync(List<UpdateDeleteMessage> deleteMessages)
    {
        var sql = "DELETE FROM \"Message\" WHERE tempId=@TempId AND chatId=@ChatId AND senderNickname=@CurrentUser";
        //return await _db.ExecuteAsync(sql, deleteMessages);
        var affectedRows = 0;
        foreach (var message in deleteMessages)
        {
            affectedRows += await _db.ExecuteAsync(sql, message);
        }
        return affectedRows;

    }

    public async Task<int> DeleteMessageForYourselfAsync(UpdateDeleteMessage deleteMessage)
    {
        var sql = "UPDATE \"Message\" SET status=1 WHERE tempId=@TempId AND chatId=@ChatId AND senderNickname=@CurrentUser";
        return await _db.ExecuteAsync(sql, deleteMessage);

    }

    public async Task<List<Message?>> GetLatestMessagesByIdAsync(int chatId, int lastMessageId)
    {
        var parameters = new { chatId, lastMessageId };
        var sql = "SELECT * FROM \"Message\"  WHERE chatId=@ChatId AND messageId > @lastMessageId";
        var messages = await _db.QueryAsync<Message?>(sql,parameters);
        return messages.ToList();

    }

    public async Task<List<Message?>> GetMessagesByDateAsync(int chatId, string startDate, string endDate)
    {
        var sql = "SELECT * FROM \"Message\" WHERE chatId=@ChatId AND created >= TO_TIMESTAMP(@startDate, 'YYYY-MM-DD HH24:MI:SS') AND created <= TO_TIMESTAMP(@endDate, 'YYYY-MM-DD HH24:MI:SS')";
        var parameters = new { chatId, startDate, endDate };
        var messages = await _db.QueryAsync<Message?>(sql, parameters);
        return messages.ToList();
    }

    public async Task<List<Message?>> GetMessagesByStatusAsync(MessageStatus messageStatus)
    {
        var sql = "SELECT * FROM \"Message\"  WHERE chatId=@ChatId AND senderNickname=@SenderNickname AND status=@status";
        var messages = await _db.QueryAsync<Message?>(sql, messageStatus);
        return messages.ToList();
    }

    public Task<Message?> ReforwardToMessageAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Message?> ReplyToMessageAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<int> SetMessageAsync(Message message)
    {
        string sql = "INSERT INTO \"Message\"  (messageId, chatId,tempId, senderNickname, messageType, messageContent, created, deleted, updated, status)" +
            "VALUES (@messageId, @ChatId,@TempId, @SenderNickname, @MessageType, @MessageContent, @Created, @Deleted, @Updated, @Status) RETURNING messageId";
        int messageId = await _db.ExecuteScalarAsync<int>(sql, message);
        return messageId;
    }
    public async Task<int> BulkInsertMessagesAsync(List<Message> messages)
    {
        //await _db.BulkInsertAsync(messages);
        
        string sql = "INSERT INTO \"Message\" (messageId ,chatId, tempId, senderNickname, messageType, messageContent, created, deleted, updated, status)" +
           "VALUES (@messageId, @ChatId, @TempId,  @SenderNickname, @MessageType,  @MessageContent, @Created, @Deleted, @Updated, @Status)";
        int rows = await _db.ExecuteAsync(sql, messages);
        return rows;
    }

    public async Task<int> UpdateMessageAsync(List<UpdateDeleteMessage> updateMessage)
    {
        var sql = "UPDATE \"Message\"  SET messageContent=@MessageContent WHERE tempId=@TempId AND chatId=@ChatId AND senderNickname=@CurrentUser";
        //return await _db.ExecuteAsync(sql, updateMessage);

        var affectedRows=0;
        foreach (var message in updateMessage)
        {
            affectedRows += await _db.ExecuteAsync(sql, message);
        }
        return affectedRows;
    }

    public async Task<int> UpdateMessageStatusAsync(MessageStatus messageStatus)
    {
        var sql = "UPDATE \"Message\"  SET status=@Status WHERE messageId=@MessageId AND chatIds=@ChatId AND senderNickname=@SenderNickname";
        return await _db.ExecuteAsync(sql, messageStatus);
    }
}
