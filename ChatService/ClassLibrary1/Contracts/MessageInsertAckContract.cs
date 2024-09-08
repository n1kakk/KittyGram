
namespace ClassLibrary1.Contracts;

//public record MessageInsertAckContract
//{
//    public Guid MessageId { get; init; } 
//    public string TempId { get; init; }
//}

public class MessageInsertAckItem
{
    public Guid MessageId { get; init; }
    public string TempId { get; init; }
}

public class MessageInsertAckContract
{
    public List<MessageInsertAckItem> InsertedMessages { get; set; }
}
