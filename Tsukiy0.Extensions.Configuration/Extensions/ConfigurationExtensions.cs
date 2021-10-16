using System;
using Microsoft.Extensions.Configuration;

namespace Tsukiy0.Extensions.Configuration.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetSection<T>(this IConfiguration configuration) where T : class
        {
            var classType = typeof(T);
            var className = classType.Name;
            return configuration.GetSection(className).Get<T>();
        }

        public static T GetSection<T>(this IConfiguration configuration, string key)
        {
            return configuration.GetSection(key).Get<T>();
        }
    }
}
