using ClassLibrary1.Models.PostgreModels.Message;

namespace ClassLibrary1.InterfaceRepository.IPostgresRepo;

public interface IMessagePostgreRepository
{
    Task<int> SetMessageAsync(Message message);
    Task<int> BulkInsertMessagesAsync(List<Message> messages);

    Task<int> UpdateMessageAsync(List<UpdateDeleteMessage> updateMessage);


    Task<int> DeleteMessageForYourselfAsync(UpdateDeleteMessage deleteMessage);
    Task<int> DeleteMessageForEveryoneAsync(List<UpdateDeleteMessage> deleteMessage);


    //Для этого нужно делать новую таблицу??? Скорее доп поле
    Task<Message?> ReplyToMessageAsync();
    Task<Message?> ReforwardToMessageAsync();


    Task<int> UpdateMessageStatusAsync(MessageStatus messageStatus);

    Task<List<Message?>> GetMessagesByStatusAsync(MessageStatus messageStatus);
    Task<List<Message?>> GetLatestMessagesByIdAsync(int conversationId, int lastMessageId);
    Task<List<Message?>> GetMessagesByDateAsync(int conversationId, string startDate, string endDate);

}
