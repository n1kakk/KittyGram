
namespace ClassLibrary1.Models;

public class MessageJS
{
    public int ChatId { get; set; }
    public string TempId { get; set; }
    public string ChatName { get; set; }
    public string SenderNickname { get; set; }
    public int MessageType { get; set; } = 0;
    public string MessageContent { get; set; }
    public DateTime Created { get; set; }
}
