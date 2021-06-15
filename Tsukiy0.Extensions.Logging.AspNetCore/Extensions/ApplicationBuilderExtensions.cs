using Tsukiy0.Extensions.Logging.AspNetCore.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Tsukiy0.Extensions.Logging.AspNetCore.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddSharedLogging(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<LogCorrelationMiddleware>();
        }
    }
}
