using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tsukiy0.Extensions.Correlation.Services;
using Tsukiy0.Extensions.Processor.Models;

namespace Tsukiy0.Extensions.Processor.Services
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

        public async Task Send(T body)
        {
            await messageQueue.Send(
                new Message<T>(
                    Header: new MessageHeader(
                        Version: 1,
                        TraceId: correlationService.TraceId,
                        Created: DateTimeOffset.UtcNow,
                        AdditionalHeaders: new Dictionary<string, string> { }
                    ),
                    Body: body
                )
            );
        }
    }
}
