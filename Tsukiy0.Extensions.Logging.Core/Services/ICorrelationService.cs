using System;

namespace Tsukiyy0.Extensions.Logging.Core.Services
{
    public interface ICorrelationService
    {
        public Guid TraceId { get; }
        public Guid SpanId { get; }
    }
}
