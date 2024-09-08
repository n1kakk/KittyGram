using ClassLibrary1.InterfaceRepository.IPostgresRepo;
using ClassLibrary1.Models.PostgreModels.Conversation;


namespace ClassLibrary1.Repository.PostgresRepo;

internal class ChatPostgreRepository : IChatPostgreRepository
{
    public Task<bool> DeleteChatForEveryoneAsync(int chatId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteChatForYoursefAsync(int chatId)
    {
        throw new NotImplementedException();
    }

    public Task<Chat?> GetChatAsync(int chatId)
    {
        throw new NotImplementedException();
    }

    public Task<int> SetChatAsync(Chat chat)
    {
        throw new NotImplementedException();
    }

    public Task<Chat?> UpdateChatNameAsync(int chatId, string newCChatName)
    {
        throw new NotImplementedException();
    }
}
