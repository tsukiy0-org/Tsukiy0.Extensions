using System;
using System.Text;
using System.Text.Json;
using NLog;
using NLog.LayoutRenderers;
using Tsukiy0.Extensions.Logging.Core.Models;
using Tsukiy0.Extensions.Logging.Core.Extensions;

namespace Tsukiy0.Adapter.NLog
{
    internal class LogLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var log = new Log(
                1,
                logEvent.Level.Ordinal * 10,
                logEvent.TimeStamp.ToUniversalTime(),
                RenderScopeId(logEvent, LoggerExtensions.TRACE_ID_KEY),
                RenderScopeId(logEvent, LoggerExtensions.SPAN_ID_KEY),
                logEvent.FormattedMessage,
                logEvent.Properties,
                RenderException(logEvent)
            );

            var json = JsonSerializer.Serialize(log);

            builder.Append(json);
        }

        private static LogException? RenderException(LogEventInfo logEvent)
        {
            if (logEvent.Exception is null)
            {
                return null;
            }

            return new LogException(
                logEvent.Exception.GetType().ToString(),
                logEvent.Exception.Message,
                logEvent.Exception.StackTrace,
                logEvent.Exception.Data
            );
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
