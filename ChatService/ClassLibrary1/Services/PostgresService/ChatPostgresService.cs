using ClassLibrary1.InterfaceRepository.IPostgresRepo;
using ClassLibrary1.InterfaceServices.IPostgresService;
using ClassLibrary1.Models.PostgreModels.Conversation;

namespace ClassLibrary1.Services.PostgresSerice;

public class ChatPostgresService : IChatPostgresService
{
    private readonly IChatPostgreRepository _chatRepo;
    public ChatPostgresService(IChatPostgreRepository chatRepo)
    {
        _chatRepo = chatRepo;
    }
    public Task<bool> DeleteChatForEveryoneAsync(int chatId)
    {
        //проверка на userrole
        throw new NotImplementedException();
    }

    public Task<bool> DeleteChatForYoursefAsync(int chatId)
    {
        //проверка на userrole, если админ - передать другому челу или просто все удалить
        throw new NotImplementedException();
    }

    public Task<Chat?> GetChatAsync(int chatId)
    {
        throw new NotImplementedException();
    }

    public async Task<Chat?> SetChatAsync(Chat chat)
    {
        var newChat = new Chat
        {
            ChatName = chat.ChatName,
            ChatType = chat.ChatType,
            Created = DateTime.UtcNow,
        };
        int chatId = await _chatRepo.SetChatAsync(newChat);
        if(chatId == 0) { return null; }
        return newChat;
    }

    public Task<Chat?> UpdateChatNameAsync(int chatId, string newChatName)
    {
        throw new NotImplementedException();
    }

    public enum ChatType
    {
        PrivateChat = 1,
        PublicChat = 2
    }
    //public enum ConvDel  вопрос
    //{
    //    False = 1,
    //    PublicConv = 2
    //}
    //public enum ConvUpd  вопрос
    //{
    //    False = 1,
    //    PublicConv = 2
    //}
}
