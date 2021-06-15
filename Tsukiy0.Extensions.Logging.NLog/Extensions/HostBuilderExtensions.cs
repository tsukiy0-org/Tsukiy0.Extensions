using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Hosting;
using NLog.LayoutRenderers;
using NLog.Layouts;
using NLog.Targets;
using Tsukiy0.Extensions.NLog.Renderers;

namespace Tsukiy0.Extensions.Logging.Nlog.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddLoggingExtensions(this IHostBuilder builder, string name)
        {
            LogManager.Configuration = BuildConfig(name);
            return builder
                .ConfigureLogging((_, logging) =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog();
        }

        private static LoggingConfiguration BuildConfig(string name)
        {
            var config = new LoggingConfiguration();
            LayoutRenderer.Register("shared-log", _ =>
            {
                var renderer = new LogLayoutRenderer(name);
                return renderer.Render(_);
            });

            var consoleTarget = new ConsoleTarget()
            {
                Layout = Layout.FromString("${shared-log}")
            };

            config.AddRuleForAllLevels(consoleTarget);

            return config;
        }
    }
}
