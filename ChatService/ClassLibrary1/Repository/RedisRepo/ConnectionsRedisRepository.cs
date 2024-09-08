using ClassLibrary1.InterfaceRepository.IRedisRepo;
using StackExchange.Redis;

namespace ClassLibrary1.Repository.RedisRepo;
public class ConnectionsRedisRepository : IConnectionsRedisRepository
{
    private readonly IDatabaseAsync _rdbAsync;
    public ConnectionsRedisRepository(ConnectionMultiplexer redis)
    {
        _rdbAsync = redis.GetDatabase();
    }

    public async Task<string?> GetConnectionIdByNicknameAsync(string nickname)
    {
        return await _rdbAsync.HashGetAsync("userConnection", nickname);
    }

    public async Task SetConnectionAsync(string nickname, string connectionId)
    {
        //await _rdbAsync.StringSetAsync($"nickname:{nickname}", connection);
        var hashEntry = new HashEntry[]
        {
        new HashEntry("connectionId", connectionId),
        new HashEntry("nickname", nickname)
        };

        await _rdbAsync.HashSetAsync($"userConnection:{connectionId}", hashEntry);
    }
    public async Task<string?> GetNicknameByConnectionIdAsync(string connectionId)
    {
        return await _rdbAsync.HashGetAsync("userConnection", connectionId);
    }

    public async Task DeleteConnectionAsync(string connectionId)
    {
        await _rdbAsync.HashDeleteAsync("userConnection", connectionId);
    }

    //public async Task RemoveLastActiveConnectionAsync(string nickname)
    //{
    //    await _rdbAsync.SortedSetRemoveAsync("last_active_connections", nickname, When.Exists);
    //}
}
