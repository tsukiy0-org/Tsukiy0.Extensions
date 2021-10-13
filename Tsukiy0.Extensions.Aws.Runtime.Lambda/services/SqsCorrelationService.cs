using System;
using Tsukiy0.Extensions.Aws.Core.Models;
using Tsukiy0.Extensions.Correlation.Services;

namespace Tsukiy0.Extensions.Aws.Runtime.Lambda
{
    public class SqsCorrelationService<T> : ICorrelationService
    {
        public Guid TraceId { get; }

        public Guid SpanId { get; }

        public SqsCorrelationService(SendMessageEnvelope<T> message)
        {
            TraceId = GetTraceId(message);
            SpanId = Guid.NewGuid();
        }

        private Guid GetTraceId(SendMessageEnvelope<T> message)
        {
            try
            {
                if (message.TraceId == Guid.Empty)
                {
                    return Guid.NewGuid();
                }

                return message.TraceId;
            }
            catch
            {
                return Guid.NewGuid();
            }
        }
    }
}
