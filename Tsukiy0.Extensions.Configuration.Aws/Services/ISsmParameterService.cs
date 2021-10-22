using System.Threading.Tasks;

namespace Tsukiy0.Extensions.Configuration.Aws.Services
{
    public interface ISsmParameterService
    {
        Task<string> Get(string key);
    }
}
