using System;
using System.Collections.Generic;

namespace Tsukiy0.Extensions.Processor.Models
{
    public class Message<T>
    {
        public MessageHeader Header { get; set; }
        public T Body { get; set; }
    }

    public class MessageHeader
    {
        public int Version { get; set; }
        public Guid TraceId { get; set; }
        public DateTimeOffset Created { get; set; }
        public IDictionary<string, string> AdditionalHeaders { get; set; }
    }
}
