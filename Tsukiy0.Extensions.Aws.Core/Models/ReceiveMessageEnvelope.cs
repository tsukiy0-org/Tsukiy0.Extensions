using System;

namespace Tsukiy0.Extensions.Aws.Core.Models
{
    public record ReceiveMessageEnvelope<T>(string ReceiptHandle, Guid TraceId, T Message);
}
