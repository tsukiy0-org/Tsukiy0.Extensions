using Tsukiy0.Extensions.Logging.AspNetCore.Middlewares;
using Tsukiy0.Extensions.Logging.AspNetCore.Services;
using Tsukiy0.Extensions.Logging.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Tsukiy0.Extensions.Logging.AspNetCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoggingExtensions(this IServiceCollection services)
        {
            return services
                .AddHttpContextAccessor()
                .AddScoped<ICorrelationService, RequestCorrelationService>()
                .AddScoped<LogCorrelationMiddleware>()
                .AddScoped<LogRequestMiddleware>();
        }
    }
}
