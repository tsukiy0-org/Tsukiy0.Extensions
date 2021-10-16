using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tsukiy0.Extensions.AspNetCore.Configs;
using Tsukiy0.Extensions.AspNetCore.Filters;
using Tsukiy0.Extensions.AspNetCore.Middlewares;
using Tsukiy0.Extensions.AspNetCore.Services;
using Tsukiy0.Extensions.Correlation.Services;
using Tsukiy0.Extensions.Configuration.Extensions;
using Tsukiy0.Extensions.Http.Constants;
using Tsukiy0.Extensions.Logging.AspNetCore.Middlewares;

namespace Tsukiy0.Extensions.AspNetCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultControllers(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(_ =>
            {
                _.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            return services;
        }

        public static IServiceCollection AddCorrelationService(this IServiceCollection services)
        {
            return services
                .AddHttpContextAccessor()
                .AddScoped<ICorrelationService, RequestCorrelationService>();
        }

        public static IServiceCollection AddDefaultLogging(this IServiceCollection services)
        {
            return services
                .AddScoped<LogCorrelationMiddleware>()
                .AddScoped<LogRequestMiddleware>();
        }

        public static IServiceCollection AddErrorHandling(this IServiceCollection services)
        {
            return services
                .AddScoped<ErrorHandlingMiddleware>();
        }

        public static IServiceCollection AddApiKeyAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureSwaggerGen(_ =>
            {
                _.AddAuthHeader(HttpHeaders.ApiKey);
            });

            return services
                .AddConfig<ApiKeyAuthConfig>(configuration)
                .AddScoped<ApiKeyAuthFilter>();
        }

        public static IServiceCollection AddDefaults(this IServiceCollection services)
        {
            services.AddDefaultControllers();
            services.AddDefaultLogging();
            services.AddErrorHandling();

            services.AddResponseCompression();

            return services;
        }
    }
}
