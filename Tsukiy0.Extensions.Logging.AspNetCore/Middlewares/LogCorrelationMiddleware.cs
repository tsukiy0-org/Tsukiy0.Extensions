using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tsukiy0.Extensions.Correlation.Services;
using Tsukiy0.Extensions.Logging.Core.Extensions;

namespace Tsukiy0.Extensions.Logging.AspNetCore.Middlewares
{
    public class LogCorrelationMiddleware : IMiddleware
    {
        private readonly ICorrelationService correlationService;

        public LogCorrelationMiddleware(ICorrelationService correlationService)
        {
            this.correlationService = correlationService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<LogCorrelationMiddleware>>();
            await logger.WithCorrelation(correlationService, async () =>
            {
                await next(context);
            });
        }
    }

}
