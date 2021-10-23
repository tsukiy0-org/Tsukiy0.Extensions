using System.Collections.Generic;
using System.Threading.Tasks;
using Tsukiy0.Extensions.Messaging.Models;

namespace Tsukiy0.Extensions.Messaging.Services
{
    public interface IMessageQueue<T>
    {
        Task Send(Message<T> message);
        Task Send(IEnumerable<Message<T>> messages);
    }
}
