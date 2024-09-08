using UserServiceDAL.Model.ElasticSearch;
using UserServiceDAL.Model.User;

namespace UserServiceDAL.InterfaceRepository
{
    public interface IElasticSearchRepository
    {
        Task<UserModelElastic?> SetUserAsync(UserModelElastic userModelElastic);
        Task<UserModelElastic?> UpdateUserNickNameAsync(UserModelElastic userModelElastic);
        Task<List<string>?> SearchNickNameAsync(string nickname);
    }
}
