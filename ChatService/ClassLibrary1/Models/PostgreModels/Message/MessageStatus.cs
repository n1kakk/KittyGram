using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Models.PostgreModels.Message;

public class MessageStatus
{
    public Guid MessageId { get; set; }
    public int ChatId { get; set; }
    public string SenderNickname { get; set; }
    public int Status { get; set; }
}
