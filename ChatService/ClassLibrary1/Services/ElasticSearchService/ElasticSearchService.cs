using ClassLibrary1.Contracts;
using ClassLibrary1.InterfaceRepository.IElasticRepo;
using ClassLibrary1.InterfaceServices.IElasticSEarchService;
using ClassLibrary1.Models.PostgreModels.Message;

namespace ClassLibrary1.Services.ElasticSearchService;

public class ElasticSearchService : IElasticSearchService
{
    private readonly IElasticSearchRepository _elasticSearchRepository;
    public ElasticSearchService(IElasticSearchRepository elasticSearchRepository)
    {
        _elasticSearchRepository = elasticSearchRepository;
    }
    public Task<List<string>> SearchMessagesAsync(string nickname)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SetMessagesAsync(List<MessageContract> messageModels)
    {
        var result = await _elasticSearchRepository.AddDocumentsAsync(messageModels, "messages");
        return result;
    }


    public async Task<bool> DeleteMessagesAsync(List<UpdateDeleteMessage> messages)
    {
        var result = await _elasticSearchRepository.DeleteMessagesAsync(messages, "messages");
        return result;
    }

    public async Task<bool> EditMessagesAsync(List<UpdateDeleteMessage> messages)
    {
        var result = await _elasticSearchRepository.EditMessagesAsync(messages, "messages");
        return result;
    }
}
