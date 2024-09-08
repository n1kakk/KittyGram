using NLog;
using UserServiceDAL.Helpers;
using UserServiceDAL.InterfaceRepository;
using UserServiceDAL.InterfaceServices;
using UserServiceDAL.Model.ElasticSearch;

namespace UserServiceDAL.Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly IElasticSearchRepository _elasticSearchRepository;
        private ILogger<ElasticSearchService> _logger;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ElasticSearchService(IElasticSearchRepository elasticSearchRepository, ILogger<ElasticSearchService> logger)
        {
            _elasticSearchRepository = elasticSearchRepository;
            _logger = logger;
        }

        public async Task<List<string>> SearchNickNameAsync(string nickname)
        {
            var result = await _elasticSearchRepository.SearchNickNameAsync(nickname);
            if (result == null)
            {
                _logger.LogError($"Failed to find user by nickname: {nickname}");
                throw new AppException($"Failed to find user");
            }
            return result;
        }

        public async Task<bool> SetUserAsync(UserModelElastic userModelElastic)
        {
            var result = await _elasticSearchRepository.SetUserAsync(userModelElastic);
            if (result == null) return false;
            return true;
        }

        public async Task<bool> UpdateUserNickNameAsync(UserModelElastic userModelElastic)
        {
            var result = await _elasticSearchRepository.UpdateUserNickNameAsync(userModelElastic);
            if (result == null) return false;
            return true;
        }
    }
}
