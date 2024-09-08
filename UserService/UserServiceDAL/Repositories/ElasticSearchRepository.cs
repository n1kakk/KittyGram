using Nest;
using UserServiceDAL.InterfaceRepository;
using UserServiceDAL.Model.ElasticSearch;

namespace UserServiceDAL.Repositories
{
    public class ElasticSearchRepository : IElasticSearchRepository
    {
        private readonly IElasticClient _elasticClient;

        public ElasticSearchRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<List<string>?> SearchNickNameAsync(string nickname)
        {
            var searchResponse = await _elasticClient.SearchAsync<UserModelElastic>(s => s
                .Index("profile")
                .Query(q => q
                      .MatchPhrasePrefix(c => c
                            .Field(p => p.NickName)
                            .Query(nickname)
                      )
                )
            );
            if (!searchResponse.IsValid) return null;

            var result = searchResponse.Documents.Select(doc => doc.NickName).ToList();

            return result;

        }

        public async Task<UserModelElastic?> SetUserAsync(UserModelElastic userModelElastic)
        {
            var indexResponse = await _elasticClient.IndexAsync(userModelElastic, idx => idx.Index("profile"));
            if (indexResponse.IsValid) return userModelElastic;
            return null;
        }

        public async Task<UserModelElastic?> UpdateUserNickNameAsync(UserModelElastic userModelElastic)
        {
            var updateResponse = await _elasticClient.UpdateAsync<UserModelElastic>(userModelElastic.Id, u => u
                .Index("profile")
                .Doc(new UserModelElastic
                {
                   Id = userModelElastic.Id,
                   NickName = userModelElastic.NickName
                })
            );
            if(updateResponse.IsValid) { return userModelElastic; }
            return null;

        }
    }
}
