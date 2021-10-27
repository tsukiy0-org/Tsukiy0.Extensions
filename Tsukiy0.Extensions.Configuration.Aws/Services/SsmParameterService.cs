using System;
using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.Extensions.Caching.Memory;

namespace Tsukiy0.Extensions.Configuration.Aws.Services
{
    public class SsmParameterService : ISsmParameterService
    {
        private readonly IAmazonSimpleSystemsManagement _client;
        private readonly IMemoryCache _cache;

        public SsmParameterService(IAmazonSimpleSystemsManagement client, IMemoryCache cache)
        {
            _client = client;
            _cache = cache;
        }

        public async Task<string?> Get(string key)
        {
            return await _cache.GetOrCreateAsync($"{nameof(SsmParameterService)}__{key}", async (_) =>
            {
                var res = await _client.GetParameterAsync(new GetParameterRequest
                {
                    Name = key,
                    WithDecryption = true
                });

                return res.Parameter.Value;
            });
        }
    }
}
