using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tsukiy0.Extensions.Processor.Services
{
    public interface IQueue<T>
    {
        Task Send(T message);
        Task Send(IEnumerable<T> messages);
    }
}
