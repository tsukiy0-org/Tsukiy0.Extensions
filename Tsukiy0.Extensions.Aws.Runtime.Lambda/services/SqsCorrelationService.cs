using System;
using System.Text.Json;
using Tsukiy0.Extensions.Aws.Core.Models;
using Tsukiy0.Extensions.Logging.Core.Services;
using static Amazon.Lambda.SQSEvents.SQSEvent;

namespace Tsukiy0.Extensions.Aws.Runtime.Lambda
{
    public class SqsCorrelationService : ICorrelationService
    {
        public Guid TraceId { get; }

        public Guid SpanId { get; }

        public SqsCorrelationService(SQSMessage message)
        {
            TraceId = GetTraceId(message);
            SpanId = Guid.NewGuid();
        }

        private Guid GetTraceId(SQSMessage message)
        {
            var raw = JsonSerializer.Deserialize<SendMessageEnvelope<object>>(message.Body);

            if (raw is null || raw.TraceId == Guid.Empty)
            {
                return Guid.NewGuid();
            }
            else
            {
                return raw.TraceId;
            }
        }
    }
}
