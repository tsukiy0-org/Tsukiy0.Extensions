using System.Text.Json;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.LayoutRenderers;
using NLog.Layouts;
using NLog.Targets;
using Microsoft.Extensions.Logging;
using NLog;
using Tsukiy0.Extensions.NLog.Renderers;
using Tsukiy0.Extensions.Logging.Models;
using Tsukiy0.Extensions.Json.Extensions;

namespace Tsukiy0.Extensions.Logging.NLog.Tests.Helpers
{
    public class MemoryLoggerHelper<T>
    {
        public readonly MemoryTarget Target;
        public readonly ILogger<T> Logger;
        public readonly string Name;

        public MemoryLoggerHelper()
        {
            Name = "TestApp";
            var config = new LoggingConfiguration();
            LayoutRenderer.Register("test-log", _ =>
            {
                var renderer = new LogLayoutRenderer(Name);
                return renderer.Render(_);
            });
            Target = new MemoryTarget()
            {
                Layout = Layout.FromString("${test-log}")
            };
            config.AddRuleForAllLevels(Target);
            var factory = new NLogLoggerFactory(
                new NLogLoggerProvider(new NLogProviderOptions(), new LogFactory(config))
            );
            Logger = factory.CreateLogger<T>();
        }

        public Log GetLog(int index)
        {
            return JsonSerializer.Deserialize<Log>(Target.Logs[index], JsonSerializerExtensions.DefaultOptions);
        }

        public Log GetFirstLog()
        {
            return GetLog(0);
        }
    }
}
