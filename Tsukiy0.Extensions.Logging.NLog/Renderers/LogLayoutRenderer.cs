using System;
using System.Text;
using System.Text.Json;

using NLog;
using NLog.LayoutRenderers;

using Tsukiy0.Extensions.Json.Extensions;
using Tsukiy0.Extensions.Logging.Extensions;
using Tsukiy0.Extensions.Logging.Models;

namespace Tsukiy0.Extensions.NLog.Renderers
{
    public class LogLayoutRenderer : LayoutRenderer
    {
        private readonly string name;

        public LogLayoutRenderer(string name)
        {
            this.name = name;
        }

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var log = new Log
            {
                Version = 1,
                Level = logEvent.Level.Ordinal * 10,
                Timestamp = logEvent.TimeStamp.ToUniversalTime(),
                Name = name,
                TraceId = RenderScopeId(logEvent, LoggerExtensions.TRACE_ID_KEY),
                SpanId = RenderScopeId(logEvent, LoggerExtensions.SPAN_ID_KEY),
                Message = logEvent.FormattedMessage,
                Context = logEvent.Properties,
                Exception = RenderException(logEvent)
            };

            var json = JsonSerializer.Serialize(log, JsonSerializerExtensions.DefaultOptions);

            builder.Append(json);
        }

        private static LogException? RenderException(LogEventInfo logEvent)
        {
            if (logEvent.Exception is null)
            {
                return null;
            }

            return new LogException
            {
                Type = logEvent.Exception.GetType().ToString(),
                Message = logEvent.Exception.Message,
                StackTrace = logEvent.Exception.StackTrace,
                Context = logEvent.Exception.Data
            };
        }

        private static Guid? RenderScopeId(LogEventInfo logEvent, string key)
        {
            var renderer = new MdlcLayoutRenderer
            {
                Item = key
            };

            var id = renderer.Render(logEvent);

            try
            {
                return new Guid(id);
            }
            catch
            {
                return null;
            }
        }
    }
}