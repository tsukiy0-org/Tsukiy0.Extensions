using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Tsukiy0.Extensions.Configuration.Aws.Exceptions;
using Tsukiy0.Extensions.Configuration.Aws.Models;

namespace Tsukiy0.Extensions.Configuration.Aws.Services
{
    public class SsmParameterConfigurationProvider : ConfigurationProvider
    {
        private readonly ISsmParameterService _ssmParameterService;
        private readonly IList<SsmParameterMap> _maps;

        public SsmParameterConfigurationProvider(ISsmParameterService ssmParameterService, IList<SsmParameterMap> maps)
        {
            _ssmParameterService = ssmParameterService;
            _maps = maps;
        }

        public override void Load()
        {
            LoadAsync().GetAwaiter().GetResult();
        }

        private async Task LoadAsync()
        {
            var tasks = _maps.Select(async map =>
            {
                var value = await _ssmParameterService.Get(map.ParameterKey);

                if (value is null)
                {
                    throw new SsmParameterNotFoundException(map);
                }

                Data.Add(map.ConfigurationKey, value);
            });

            await Task.WhenAll(tasks);
        }
    }
}