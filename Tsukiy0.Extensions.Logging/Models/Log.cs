using System;

namespace Tsukiy0.Extensions.Logging.Models
{
    public record Log
    {
        public int Version { get; init; }
        public int Level { get; init; }
        public DateTimeOffset Timestamp { get; init; }
        public string Name { get; init; }
        public Guid? TraceId { get; init; }
        public Guid? SpanId { get; init; }
        public string Message { get; init; }
        public dynamic Context { get; init; }
        public LogException? Exception { get; init; }
    }
}
