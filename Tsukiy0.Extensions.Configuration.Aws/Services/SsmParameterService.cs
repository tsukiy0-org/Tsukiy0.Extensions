using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.Extensions.Caching.Memory;

namespace Tsukiy0.Extensions.Configuration.Aws.Services
{
    public class SsmParameterService : IParameterService
    {
        private readonly static IMemoryCache CACHE = new MemoryCache(new MemoryCacheOptions());

        private readonly IAmazonSimpleSystemsManagement _client;

        public SsmParameterService(IAmazonSimpleSystemsManagement client)
        {
            _client = client;
        }

        public async Task<string> Get(string key)
        {
            return await CACHE.GetOrCreateAsync($"{nameof(SsmParameterService)}__{key}", async (_) =>
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
