using ClassLibrary1.Models.PostgreModels.Message;


namespace ClassLibrary1.InterfaceRepository.IElasticRepo;
public interface IElasticSearchRepository
{
    //Task<UserModelElastic?> UpdateUserNickNameAsync(UserModelElastic userModelElastic);
    //Task<List<string>?> SearchMessagesAsync(string nickname);
    Task<bool> AddDocumentsAsync<T>(IEnumerable<T> documents, string indexName) where T : class;
    Task<bool> DeleteMessagesAsync(List<UpdateDeleteMessage> deleteMessages, string indexName);
    Task<bool> EditMessagesAsync(List<UpdateDeleteMessage> editMessages, string indexName);
}
