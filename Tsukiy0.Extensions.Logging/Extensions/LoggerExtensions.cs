using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Tsukiy0.Extensions.Correlation.Services;

namespace Tsukiy0.Extensions.Logging.Extensions
{
    public static class LoggerExtensions
    {
        public const string TRACE_ID_KEY = "TRACE_ID";
        public const string SPAN_ID_KEY = "SPAN_ID";

        public static T WithCorrelation<T>(this ILogger logger, ICorrelationService correlationService, Func<T> fn)
        {
            using (logger.BeginScope(new[] {
                new KeyValuePair<string, object>(TRACE_ID_KEY, correlationService.TraceId),
                new KeyValuePair<string, object>(SPAN_ID_KEY, correlationService.SpanId)
            }))
            {
                return fn();
            }
        }

        public static void WithCorrelation(this ILogger logger, ICorrelationService correlationService, Action fn)
        {
            using (logger.BeginScope(new[] {
                new KeyValuePair<string, object>(TRACE_ID_KEY, correlationService.TraceId),
                new KeyValuePair<string, object>(SPAN_ID_KEY, correlationService.SpanId)
            }))
            {
                fn();
            }
        }
    }
}
