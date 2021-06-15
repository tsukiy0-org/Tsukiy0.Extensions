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
        public static IHostBuilder AddLoggingExtensions(this IHostBuilder builder)
        {
            LogManager.Configuration = BuildConfig();
            return builder
                .ConfigureLogging((_, logging) =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog();
        }

        private static LoggingConfiguration BuildConfig()
        {
            var config = new LoggingConfiguration();
            LayoutRenderer.Register<LogLayoutRenderer>("shared-log");

            var consoleTarget = new ConsoleTarget()
            {
                Layout = Layout.FromString("${shared-log}")
            };

            config.AddRuleForAllLevels(consoleTarget);

            return config;
        }
    }
}
