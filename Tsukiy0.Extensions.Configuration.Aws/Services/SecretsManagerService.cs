using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Caching.Memory;
using Tsukiy0.Extensions.Json.Extensions;

namespace Tsukiy0.Extensions.Configuration.Aws.Services
{
    public class SecretsManagerService : ISecretsManagerService
    {
        private readonly IAmazonSecretsManager _client;
        private readonly IMemoryCache _cache;

        public SecretsManagerService(IAmazonSecretsManager client, IMemoryCache cache)
        {
            _client = client;
            _cache = cache;
        }

        public async Task<IDictionary<string, string>> Get(string key)
        {
            return await _cache.GetOrCreateAsync(key, async (_) =>
            {
                var res = await _client.GetSecretValueAsync(new GetSecretValueRequest
                {
                    SecretId = key
                });

                return JsonSerializer.Deserialize<IDictionary<string, string>>(res.SecretString, JsonSerializerExtensions.DefaultOptions);
            });
        }
    }
}
