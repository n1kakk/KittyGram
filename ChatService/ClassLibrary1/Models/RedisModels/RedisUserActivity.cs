using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Models.RedisUserActivity;

public class RedisUserActivity
{
    public string Nickname {  get; set; }
    public DateTime LastActivity { get; set; }
}
