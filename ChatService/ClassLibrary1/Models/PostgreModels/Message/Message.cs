namespace ClassLibrary1.Models.PostgreModels.Message;
public class Message
{
    public Guid MessageId { get; set; }
    public int ChatId { get; set; }
    public string TempId { get; set; }
    public string SenderNickname { get; set; }
    public int MessageType { get; set; }
    public string MessageContent { get; set; }
    public DateTime Created { get; set; }
    public int Deleted { get; set; } = 0;
    public int Updated { get; set; } = 0;
    public int Status { get; set; } = 0;
}

