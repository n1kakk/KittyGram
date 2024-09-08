using ClassLibrary1.InterfaceRepository.IRedisRepo;
using ClassLibrary1.Models.RedisUserActivity;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Repository.RedisRepo;

public class LastActiveRedisRepository : ILastActiveRedisRepository
{
    private readonly string sortedSetKey = "userActivity";
    private readonly IDatabaseAsync _rdbAsync;

    public LastActiveRedisRepository(ConnectionMultiplexer redis)
    {
        _rdbAsync = redis.GetDatabase();
    }

    public Task DeleteUserActivityAsync(string nickname)
    {
        throw new NotImplementedException();
    }

    public async Task<DateTime?> GetUserActivityByNicknameAsync(string nickname)
    {
        double? score = await _rdbAsync.SortedSetScoreAsync(sortedSetKey, nickname);

        // Если score равно null, значит пользователя с данным никнеймом нет в sorted set
        if (score.HasValue)
        {
            return new DateTime((long)score.Value);
        }
        
        return null;
    }

    public async Task SetUserActivityAsync(RedisUserActivity userActivity)
    {
        await _rdbAsync.SortedSetAddAsync(sortedSetKey, userActivity.Nickname, userActivity.LastActivity.Ticks);
    }
}
