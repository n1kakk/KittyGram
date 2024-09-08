
namespace ClassLibrary1.Models.PostgreModels.Message
{
    public class UpdateDeleteMessage
    {
        public Guid MessageId { get; set; }
        public string TempId { get; set; }
        public string CurrentUser { get; set; }
        public int ChatId { get; set; }
        public string? MessageContent { get; set; }
    }
}
