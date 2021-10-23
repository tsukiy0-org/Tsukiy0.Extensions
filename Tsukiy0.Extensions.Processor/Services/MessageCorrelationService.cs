using System;
using Tsukiy0.Extensions.Correlation.Services;
using Tsukiy0.Extensions.Messaging.Models;

namespace Tsukiy0.Extensions.Processor.Services
{
    public class MessageCorrelationService : ICorrelationService
    {
        public Guid TraceId { get; }
        public Guid SpanId { get; }

        public MessageCorrelationService(MessageHeader header)
        {
            SpanId = Guid.NewGuid();
            TraceId = GetTraceId(header);
        }

        private static Guid GetTraceId(MessageHeader header)
        {
            if (header.TraceId == default)
            {
                return Guid.NewGuid();
            }

            return header.TraceId;
        }
    }
}
