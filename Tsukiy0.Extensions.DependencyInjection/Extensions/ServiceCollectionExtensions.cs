using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tsukiy0.Extensions.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddConfig<T>(this IServiceCollection services, IConfiguration configuration, string key) where T : class
        {
            return services.AddScoped((_) =>
            {
                return configuration.GetSection<T>(key);
            });
        }

        public static IServiceCollection AddConfig<T>(this IServiceCollection services, IConfiguration configuration) where T : class
        {
            return services.AddScoped((_) =>
            {
                return configuration.GetSection<T>();
            });
        }
    }
}
