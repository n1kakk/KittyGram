using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Contracts;

public record UpdateMessageContract
{
    public Guid MessageId { get; init; }
    public string TempId { get; init; }
    public string CurrentUser { get; init; }
    public int ChatId { get; init; }
    public string? MessageContent { get; init; }
}
