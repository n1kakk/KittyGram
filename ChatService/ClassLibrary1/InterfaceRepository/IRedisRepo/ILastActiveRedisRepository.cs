using ClassLibrary1.Models.RedisUserActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.InterfaceRepository.IRedisRepo;

public interface ILastActiveRedisRepository
{
    Task SetUserActivityAsync(RedisUserActivity userActivity);
    Task<DateTime?> GetUserActivityByNicknameAsync(string nickname);
    //Task<List<string?>> GetUserActivityByDateAsync(DateTime dateRange);
    //Task<List<string?>> DeleteUserActivityByDateAsync(DateTime dateRange); //удалять сразу стопкой, параметр - ??
    Task DeleteUserActivityAsync(string nickname);
}
