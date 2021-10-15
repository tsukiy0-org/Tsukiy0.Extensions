using System.Threading.Tasks;
using Tsukiy0.Extensions.Processor.Models;

namespace Tsukiy0.Extensions.Processor.Services
{
    public interface IMessageQueue<T>
    {
        Task Send(params Message<T>[] message);
    }
}
