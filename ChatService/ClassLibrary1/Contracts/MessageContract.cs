namespace ClassLibrary1.Contracts;

public record MessageContract
{
    public Guid MessageId { get; init; } = Guid.NewGuid();
    public string TempId {  get; init; }
    public int ChatId { get; init; }
    public string SenderNickname { get; init; }
    public int MessageType { get; init; }
    public string MessageContent { get; init; }
    public DateTime Created { get; init; }


    //Нужно исправить bool на int
    public int Deleted { get; init; } = 0;
    public int Updated { get; init; } = 0;


    public int Status { get; init; } = 0;
}

