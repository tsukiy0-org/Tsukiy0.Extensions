using System;
using System.Collections.Generic;

namespace Tsukiy0.Extensions.Processor.Models
{
    public record Message<T>(
        MessageHeader Header,
        T Body
    );

    public record MessageHeader(
        int Version,
        Guid TraceId,
        DateTimeOffset Created,
        IDictionary<string, string> AdditionalHeaders
    );
}
