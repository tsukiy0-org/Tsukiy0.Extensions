using System.Collections.Generic;
using Amazon.SecretsManager;
using Amazon.SimpleSystemsManagement;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Tsukiy0.Extensions.Configuration.Aws.Models;
using Tsukiy0.Extensions.Configuration.Aws.Services;

namespace Tsukiy0.Extensions.Configuration.Aws.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddSsmParameterConfiguration(this IConfigurationBuilder builder, IList<SsmParameterMap> maps)
        {
            return builder.Add(new SsmParameterConfigurationSource(maps));
        }

        public static IConfigurationBuilder AddSecretsManagerConfiguration(this IConfigurationBuilder builder, IList<SecretsManagerMap> maps)
        {
            return builder.Add(new SecretsManagerConfigurationSource(maps));
        }

        private class SsmParameterConfigurationSource : IConfigurationSource
        {
            private readonly static IMemoryCache CACHE = new MemoryCache(new MemoryCacheOptions());
            private readonly IList<SsmParameterMap> _maps;

            public SsmParameterConfigurationSource(IList<SsmParameterMap> maps)
            {
                _maps = maps;
            }

            public IConfigurationProvider Build(IConfigurationBuilder builder)
            {
                var service = new SsmParameterService(new AmazonSimpleSystemsManagementClient(), CACHE);
                return new SsmParameterConfigurationProvider(service, _maps);
            }
        }

        private class SecretsManagerConfigurationSource : IConfigurationSource
        {
            private readonly static IMemoryCache CACHE = new MemoryCache(new MemoryCacheOptions());
            private readonly IList<SecretsManagerMap> _maps;

            public SecretsManagerConfigurationSource(IList<SecretsManagerMap> maps)
            {
                _maps = maps;
            }

            public IConfigurationProvider Build(IConfigurationBuilder builder)
            {
                var service = new SecretsManagerService(new AmazonSecretsManagerClient(), CACHE);
                return new SecretsManagerConfigurationProvider(service, _maps);
            }
        }
    }

}
