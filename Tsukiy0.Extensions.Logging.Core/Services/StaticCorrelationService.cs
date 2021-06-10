using System;

namespace Tsukiy0.Extensions.Logging.Core.Services
{
    public class StaticCorrelationService : ICorrelationService
    {
        public Guid TraceId { get; }

        public Guid SpanId { get; }

        public StaticCorrelationService(Guid traceId, Guid spanId)
        {
            TraceId = traceId;
            SpanId = spanId;
        }
    }
}
