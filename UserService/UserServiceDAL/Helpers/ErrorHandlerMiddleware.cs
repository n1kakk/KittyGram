using NLog;
using System.Net;
using System.Text.Json;

namespace UserServiceDAL.Helpers
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ErrorHandlerMiddleware> _logger;
        private static readonly Logger Loger = LogManager.GetCurrentClassLogger();
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        //принимает объект HttpContext, который представляет контекст текущего HTTP-запроса и содержит информацию о запросе и ответе.
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                switch (error)
                {
                    case AppException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:

                        _logger.LogError(error, error.Message);
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                //error не равен null, то error.Message будет возвращено. Если error равен null, то выражение вернет null без выброса исключения; анонимный тип
                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
