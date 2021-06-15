using System;

namespace Tsukiy0.Extensions.Logging.Core.Models
{
    public record Log(
        int Version,
        int Level,
        DateTimeOffset Timestamp,
        String Name,
        Guid? TraceId,
        Guid? SpanId,
        string Message,
        dynamic Context,
        LogException? Exception
    );
}
