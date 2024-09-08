using ClassLibrary1.InterfaceRepository.IElasticRepo;
using ClassLibrary1.Models.PostgreModels.Message;
using Nest;

namespace ClassLibrary1.Repository.ElasticRepo;

public class ElasticSearchRepository : IElasticSearchRepository
{
    private readonly IElasticClient _elasticClient;

    public ElasticSearchRepository(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<bool> AddDocumentsAsync<T>(IEnumerable<T> documents, string indexName) where T : class
    {
        var response = await _elasticClient.BulkAsync(b => b.Index(indexName).IndexMany(documents));
        if (response.IsValid) return true;
        return false;
    }

    public Task<List<string>?> SearchMessagesAsync(string nickname)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteMessagesAsync(List<UpdateDeleteMessage> deleteMessages, string indexName)
    {
        foreach (var message in deleteMessages)
        {
            if (message.MessageId != Guid.Empty)
            {
                var response = await _elasticClient.DeleteByQueryAsync<UpdateDeleteMessage>(q => q
                    .Index(indexName) 
                    .Query(rq => rq
                        .Term(t => t
                            .Field(f => f.TempId.Suffix("keyword")) 
                            .Value(message.TempId)
                        )
                    )
                );
                if (!response.IsValid) return false;
            }
        }
        return true;
    }

    public async Task<bool> EditMessagesAsync(List<UpdateDeleteMessage> editMessages, string indexName)
    {
        foreach (var message in editMessages)
        {
            var scriptParams = new Dictionary<string, object> { { "newMessageContent", message.MessageContent } };
            var response = await _elasticClient.UpdateByQueryAsync<UpdateDeleteMessage>(q => q
                .Index(indexName)
                .Query(rq => rq
                    .Term(t => t
                        .Field(f => f.TempId.Suffix("keyword"))
                        .Value(message.TempId)
                        )
                    )
                  .Script(s => s
                        .Source("ctx._source.messageContent  = params.newMessageContent;")
                        .Params(scriptParams)
                        )
                 );
            if (!response.IsValid) return false;
        }
        return true;
    }
}
