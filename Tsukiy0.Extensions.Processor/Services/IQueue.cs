using System.Threading.Tasks;

namespace Tsukiy0.Extensions.Processor.Services
{
    public interface IQueue<T>
    {
        Task Send(T body);
    }
}
