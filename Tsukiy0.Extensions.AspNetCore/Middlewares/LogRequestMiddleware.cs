using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Tsukiy0.Extensions.Logging.AspNetCore.Middlewares
{
    public class LogRequestMiddleware : IMiddleware
    {
        private readonly ILogger<LogRequestMiddleware> logger;

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
                    "Transaction {request} {response}",
                    new
                    {
                        Host = context.Request.Host.Value,
                        Scheme = context.Request.Scheme,
                        Protocol = context.Request.Protocol,
                        Method = context.Request.Method,
                        Path = context.Request.Path.Value,
                        Query = context.Request.QueryString.Value,
                        ContentType = context.Request.ContentType,
                        ContentLength = context.Request.ContentLength
                    },
                    new
                    {
                        StatusCode = context.Response.StatusCode,
                        ContentType = context.Response.ContentType,
                        ContentLength = context.Response.ContentLength
                    }
                );
            }
        }
    }
}