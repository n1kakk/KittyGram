using UserServiceDAL.Model.ElasticSearch;

namespace UserServiceDAL.InterfaceServices
{
    public interface IElasticSearchService
    {
        Task<List<string>> SearchNickNameAsync(string nickname);
        Task<bool> SetUserAsync(UserModelElastic userModelElastic);
        Task<bool> UpdateUserNickNameAsync(UserModelElastic userModelElastic);
    }
}
