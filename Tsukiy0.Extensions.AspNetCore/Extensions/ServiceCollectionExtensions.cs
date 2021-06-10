using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tsukiy0.Extensions.AspNetCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfig<T>(this IServiceCollection services, IConfiguration configuration) where T : class
        {
            var name = typeof(T).Name;
            return services.AddScoped((_) =>
            {
                return configuration.GetSection(name)
                    .Get<T>();
            });
        }
    }
}
