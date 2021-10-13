using System;

namespace Tsukiy0.Extensions.Correlation.Services
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
