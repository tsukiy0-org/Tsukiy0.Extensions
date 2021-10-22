using System.Threading.Tasks;

namespace Tsukiy0.Extensions.Configuration.Aws
{
    public interface IParameterService
    {
        Task<string> Get(string key);
    }
}
