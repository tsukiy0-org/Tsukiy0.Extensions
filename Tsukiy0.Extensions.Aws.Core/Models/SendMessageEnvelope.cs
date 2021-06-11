using System;

namespace Tsukiy0.Extensions.Aws.Core.Models
{
    public record SendMessageEnvelope<T>(Guid TraceId, T Message);
}
