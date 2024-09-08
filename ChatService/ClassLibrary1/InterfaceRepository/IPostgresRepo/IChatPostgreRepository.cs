using ClassLibrary1.Models.PostgreModels.Conversation;


namespace ClassLibrary1.InterfaceRepository.IPostgresRepo;

public interface IChatPostgreRepository
{
    Task<int> SetChatAsync(Chat chat);


    Task<Chat?> GetChatAsync(int chatId);


    Task<Chat?> UpdateChatNameAsync(int chatId, string newChatName);


    Task<bool> DeleteChatForYoursefAsync(int chatId);
    Task<bool> DeleteChatForEveryoneAsync(int chatId);
}
