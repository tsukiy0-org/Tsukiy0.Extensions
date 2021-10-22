using System.Threading.Tasks;

namespace Tsukiy0.Extensions.Configuration.Aws.Services
{
    public interface IParameterService
    {
        Task<string> Get(string key);
    }
}
