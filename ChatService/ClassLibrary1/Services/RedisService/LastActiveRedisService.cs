using ClassLibrary1.InterfaceRepository.IRedisRepo;
using ClassLibrary1.InterfaceServices.IRedisService;
using ClassLibrary1.Models.RedisUserActivity;


namespace ClassLibrary1.Services.RedisService;

public class LastActiveRedisService : ILastActiveRedisService
{
    private readonly ILastActiveRedisRepository _rdbRepo;
    public LastActiveRedisService(ILastActiveRedisRepository rdbRepo)
    {
        _rdbRepo = rdbRepo;   
    }
    public async Task DeleteUserActivityAsync(string nickname)
    {
        await _rdbRepo.DeleteUserActivityAsync(nickname);
    }

    public async Task<DateTime?> GetUserActivityByNicknameAsync(string nickname)
    {
        var result = await _rdbRepo.GetUserActivityByNicknameAsync(nickname);
        return result;
    }

    public async Task SetUserActivityAsync(RedisUserActivity userActivity)
    {
        await _rdbRepo.SetUserActivityAsync(userActivity);
    }
}
