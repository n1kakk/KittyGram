using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Helpers;

public class AppException:Exception
{
    public AppException() : base() { } //конструктор
    public AppException(string message) : base(message) { } //конструктор, который принимает сообщение
    public AppException(string message, params object[] args) :
        base(String.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}
