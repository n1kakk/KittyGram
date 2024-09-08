using System.Globalization;

namespace UserServiceDAL.Helpers
{
    public class AppException : Exception
    {
        public AppException() : base() { } //конструктор
        public AppException(string message) : base(message) { } //конструктор, который принимает сообщение
        public AppException(string message, params object[] args):
            base(String.Format(CultureInfo.CurrentCulture, message, args))
            {
            }
    }
}
