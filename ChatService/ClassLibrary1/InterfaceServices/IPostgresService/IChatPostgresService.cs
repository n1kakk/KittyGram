using ClassLibrary1.Models.PostgreModels.Conversation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.InterfaceServices.IPostgresService;

public interface IChatPostgresService
{
    Task<Chat?> SetChatAsync(Chat chat);

    Task<Chat?> GetChatAsync(int chatId);

    Task<Chat?> UpdateChatNameAsync(int chatId, string newChatName);

    Task<bool> DeleteChatForYoursefAsync(int chatId);
    Task<bool> DeleteChatForEveryoneAsync(int chatId);
}
