using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tsukiy0.Extensions.Configuration.Aws.Services
{
    public interface ISecretsManagerService
    {
        Task<IDictionary<string, string>> Get(string key);
    }
}