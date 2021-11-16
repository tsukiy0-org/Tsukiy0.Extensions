using System;
using System.Collections.Generic;

namespace Tsukiy0.Extensions.Messaging.Models
{
    public record Message<T>
    {
        public MessageHeader Header { get; init; }
        public T Body { get; init; }
    }

    public record MessageHeader
    {
        public int Version { get; init; }
        public Guid TraceId { get; init; }
        public DateTimeOffset Created { get; init; }
        public IDictionary<string, string> AdditionalHeaders { get; init; }
    };
}