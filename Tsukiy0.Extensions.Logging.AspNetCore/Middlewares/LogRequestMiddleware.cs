using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Tsukiy0.Extensions.Logging.AspNetCore.Middlewares
{
    public class LogRequestMiddleware : IMiddleware
    {
        private ILogger<LogRequestMiddleware> logger;

        public LogRequestMiddleware(ILogger<LogRequestMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            finally
            {
                logger.LogInformation(
                    "Request {method} {url} => {statusCode}",
                    context.Request.Method,
                    context.Request.Path.Value,
                    context.Response.StatusCode);
            }
        }
    }
}
