using System.Collections.Generic;
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
    }

}
