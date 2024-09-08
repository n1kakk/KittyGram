using ClassLibrary1.Contracts;
using ClassLibrary1.Models.PostgreModels.Message;


namespace ClassLibrary1.InterfaceServices.IElasticSEarchService;
public interface IElasticSearchService
{
    Task<List<string>> SearchMessagesAsync(string nickname);
    Task<bool> SetMessagesAsync(List<MessageContract> messageModels);
    //Task<bool> UpdateUserNickNameAsync(MessageModel messageModel);
    Task<bool> DeleteMessagesAsync(List<UpdateDeleteMessage> messages);
    Task<bool> EditMessagesAsync(List<UpdateDeleteMessage> messages);
}
