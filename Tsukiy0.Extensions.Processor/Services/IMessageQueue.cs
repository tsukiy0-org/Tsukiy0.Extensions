using System.Collections.Generic;
using System.Threading.Tasks;
using Tsukiy0.Extensions.Processor.Models;

namespace Tsukiy0.Extensions.Processor.Services
{
    public interface IMessageQueue<T>
    {
        Task Send(Message<T> message);
        Task Send(IEnumerable<Message<T>> messages);
    }
}
