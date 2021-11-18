using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Tsukiy0.Extensions.Correlation.Services;
using Tsukiy0.Extensions.Messaging.Models;

namespace Tsukiy0.Extensions.Messaging.Services
{
    public abstract class DefaultQueue<T> : IQueue<T>
    {
        private readonly IMessageQueue<T> messageQueue;
        private readonly ICorrelationService correlationService;

        public DefaultQueue(IMessageQueue<T> messageQueue, ICorrelationService correlationService)
        {
            this.messageQueue = messageQueue;
            this.correlationService = correlationService;
        }

        public async Task Send(IEnumerable<T> messages)
        {
            await messageQueue.Send(
                messages.Select(_ =>
                    new Message<T>
                    {
                        Header = new MessageHeader
                        {
                            Version = 1,
                            TraceId = correlationService.TraceId,
                            Created = DateTimeOffset.UtcNow,
                            AdditionalHeaders = new Dictionary<string, string> { }
                        },
                        Body = _
                    }
                )
            );
        }

        public async Task Send(T message)
        {
            await Send(new List<T> { message });
        }
    }
}