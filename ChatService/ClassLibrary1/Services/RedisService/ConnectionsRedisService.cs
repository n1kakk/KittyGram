using ClassLibrary1.InterfaceRepository.IRedisRepo;
using ClassLibrary1.InterfaceServices.IRedisService;

namespace ClassLibrary1.Services.RedisService;
public class ConnectionsRedisService : IConnectionsRedisService
{
    private readonly IConnectionsRedisRepository _rdbRepo;
    public ConnectionsRedisService(IConnectionsRedisRepository rdbRepo)
    {
        _rdbRepo = rdbRepo;
    }

    public async Task DeleteConnectionAsync(string connectionId)
    {
        await _rdbRepo.DeleteConnectionAsync(connectionId);
    }

    public async Task<string?> GetConnectionIdByNicknameAsync(string nickname)
    {
        return await _rdbRepo.GetConnectionIdByNicknameAsync(nickname);
    }
    public async Task<string?> GetNicknameByConnectionIdAsync(string connectionId)
    {
        return await _rdbRepo.GetNicknameByConnectionIdAsync(connectionId);
    }

    public async Task SetConnectionAsync(string nickname, string connectionId)
    {
        await _rdbRepo.SetConnectionAsync(nickname, connectionId);
    }
}
