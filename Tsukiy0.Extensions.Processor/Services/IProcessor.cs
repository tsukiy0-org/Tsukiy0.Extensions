using System.Threading.Tasks;

using Tsukiy0.Extensions.Messaging.Models;

namespace Tsukiy0.Extensions.Processor.Services
{
    public interface IProcessor<T>
    {
        Task Run(Message<T> message);
    }
}