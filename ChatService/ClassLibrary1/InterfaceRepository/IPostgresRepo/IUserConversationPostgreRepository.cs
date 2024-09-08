using ClassLibrary1.Models.PostgreModels.UserConversation;

namespace ClassLibrary1.InterfaceRepository.IPostgresRepo;

public interface IUserConversationPostgreRepository
{
    Task<bool> SetSenderAsync(UserChatModel userConversation);


    Task<List<UserChatModel?>> GetSenderConversationsAsync(string nickname);
    Task<List<UserChatModel?>> GetAllConversationSendersAndRolesAsync(int conversationId);
    Task<UserChatModel?> GetUserRoleAsync(UserChat userConv);
    Task<UserChatModel?> GetConversationSenderAsync(UserChat userConv);


    Task<UserChatModel?> UpdateUserRoleAsync(UserChat userConv);


    Task<bool> DeleteSenderAsync(UserChat userConv);
    Task<bool> DeleteConversationAsync(UserChat userConv);
    Task<bool> DeleteConversationForEveryoneAsync(int conversationId);
}
