using AutoMapper;
using ClassLibrary1.Contracts;
using ClassLibrary1.InterfaceRepository.IPostgresRepo;
using ClassLibrary1.InterfaceServices.IElasticSEarchService;
using ClassLibrary1.InterfaceServices.IPostgresService;
using ClassLibrary1.Models;
using ClassLibrary1.Models.PostgreModels.Message;
using ClassLibrary1.Publishers;


namespace ClassLibrary1.Services.PostgresService;

public class MessagePostgresService : IMessagePostgresService
{
    private readonly IMessagePostgreRepository _messagePostreRepo;
    private readonly IBasePostgresRepository _basePostgresRepository;
    private readonly IElasticSearchService _elasticSearchService;
    private readonly IMapper _mapper;
    private readonly MessageInsertPublisher _messageInsertPublisher;
    public MessagePostgresService(IMessagePostgreRepository messagePostgreRepository, IBasePostgresRepository basePostgresRepository,
        IElasticSearchService elasticSearchService, IMapper mapper, MessageInsertPublisher messageInsertPublisher)
    {
        _messagePostreRepo = messagePostgreRepository;
        _basePostgresRepository = basePostgresRepository;
        _elasticSearchService = elasticSearchService;
        _mapper = mapper;
        _messageInsertPublisher = messageInsertPublisher;
    }
    public async Task BulkInsertMessagesAsync(List<Message> messages)
    {
        int insertRows = await _messagePostreRepo.BulkInsertMessagesAsync(messages);
        if (insertRows != 0)
        {
            List<MessageInsertAckContract> messagesAck = _mapper.Map<List<MessageInsertAckContract>>(messages);
           // await _messageInsertPublisher.PublishMessageInsertAcknowlegeAsync(messagesAck);
        }

    }

    public async Task<int> DeleteMessageForEveryoneAsync(List<UpdateDeleteMessage> deleteMessage)
    {
        return await _messagePostreRepo.DeleteMessageForEveryoneAsync(deleteMessage);
    }

    public Task<int> DeleteMessageForYourselfAsync(UpdateDeleteMessage deleteMessage)
    {
        throw new NotImplementedException();
    }

    public Task<List<Message?>> GetLatestMessagesByIdAsync(int chatId, int lastMessageId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Message?>> GetMessagesByDateAsync(int chatId, DateTime dateTime)
    {
        DateTime plusDays = dateTime.AddDays(-1);
        DateTime minusDays = dateTime.AddDays(1);
        string endDate = minusDays.ToString("yyyy.MM.dd");
        string startDate = plusDays.ToString("yyyy.MM.dd");

        return await _messagePostreRepo.GetMessagesByDateAsync(chatId, startDate, endDate);
    }

    public async Task<List<Message?>> GetMessagesForMonthAsync(int chatId, DateTime dateTime)
    {
        DateTime plusDays = dateTime.AddDays(-15);
        DateTime minusDays = dateTime.AddDays(15);
        string endDate = minusDays.ToString("yyyy.MM.dd");
        string startDate = plusDays.ToString("yyyy.MM.dd");

        return await _messagePostreRepo.GetMessagesByDateAsync(chatId, startDate, endDate);
    }

    public Task<List<Message?>> GetMessagesByStatusAsync(MessageStatus messageStatus)
    {
        throw new NotImplementedException();
    }



     
    public Task<Message?> ReforwardToMessageAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Message?> ReplyToMessageAsync()
    {
        throw new NotImplementedException();
    }




    public async Task<Message?> SetMessageAsync(MessageJS message)
    {
        var messageDb = _mapper.Map<Message>(message);
        var messageId = await _messagePostreRepo.SetMessageAsync(messageDb);

        if (messageId==0) return null;

        return messageDb;
    }

    public async Task<int> UpdateMessageAsync(List<UpdateDeleteMessage> updateMessage)
    {
        int inserRows = await _messagePostreRepo.UpdateMessageAsync(updateMessage);
       return inserRows;
    }

    public Task<int> UpdateMessageStatusAsync(MessageStatus messageStatus)
    {
        throw new NotImplementedException();
    }
}
