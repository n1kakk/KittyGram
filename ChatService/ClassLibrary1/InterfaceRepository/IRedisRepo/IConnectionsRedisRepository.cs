using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.InterfaceRepository.IRedisRepo;

public interface IConnectionsRedisRepository
{
    Task SetConnectionAsync(string nickname, string connection);
    Task<string?> GetConnectionIdByNicknameAsync(string nickname);
    Task<string?> GetNicknameByConnectionIdAsync(string connectionId);
    Task DeleteConnectionAsync(string connectionId);
}
