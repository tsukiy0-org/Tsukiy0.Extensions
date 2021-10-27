using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Tsukiy0.Extensions.Configuration.Aws.Exceptions;
using Tsukiy0.Extensions.Configuration.Aws.Models;

namespace Tsukiy0.Extensions.Configuration.Aws.Services
{
    public class SecretsManagerConfigurationProvider : ConfigurationProvider
    {
        private readonly ISecretsManagerService _secretsManagerService;
        private readonly IList<SecretsManagerMap> _maps;

        public SecretsManagerConfigurationProvider(ISecretsManagerService ssmParameterService, IList<Models.SecretsManagerMap> maps)
        {
            _secretsManagerService = ssmParameterService;
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
                var secrets = await _secretsManagerService.Get(map.SecretKey);
                secrets.TryGetValue(map.SecretField, out string value);

                if (value is null)
                {
                    throw new SecretNotFoundException(map);
                }

                Data.Add(map.ConfigurationKey, value);
            });

            await Task.WhenAll(tasks);
        }
    }
}
