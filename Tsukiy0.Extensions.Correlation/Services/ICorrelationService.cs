using System;

namespace Tsukiy0.Extensions.Correlation.Services
{
    public interface ICorrelationService
    {
        public Guid TraceId { get; }
        public Guid SpanId { get; }
    }
}