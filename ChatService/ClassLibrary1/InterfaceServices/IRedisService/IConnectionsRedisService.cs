using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.InterfaceServices.IRedisService;

public interface IConnectionsRedisService
{
    Task SetConnectionAsync(string nickname, string connectionId);
    Task<string?> GetConnectionIdByNicknameAsync(string nickname);
    Task<string?> GetNicknameByConnectionIdAsync(string connectionId);
    Task DeleteConnectionAsync(string connectionId);
}
