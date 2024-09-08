

using ClassLibrary1.Models.RedisUserActivity;

namespace ClassLibrary1.InterfaceServices.IRedisService;

public interface ILastActiveRedisService
{
    Task SetUserActivityAsync(RedisUserActivity userActivity);
    Task<DateTime?> GetUserActivityByNicknameAsync(string nickname);
    //Task<List<string?>> GetUserActivityByDateAsync(DateTime dateRange);
    //Task<List<string?>> DeleteUserActivityByDateAsync(DateTime dateRange); //удалять сразу стопкой, параметр - ??
    Task DeleteUserActivityAsync(string nickname);
}
