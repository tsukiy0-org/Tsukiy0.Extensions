using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Layouts;
using NLog.Targets;
using Tsukiy0.Adapter.NLog;

namespace Tsukiy0.Extensions.Logging.NLog.Extensions
{
    public static class LoggingConfigurationExtensions
    {
        public static LoggingConfiguration AddStandardLog(this LoggingConfiguration config)
        {
            LayoutRenderer.Register<LogLayoutRenderer>("standard-log");
            var consoleTarget = new ConsoleTarget()
            {
                Layout = Layout.FromString("${standard-log}")
            };
            config.AddRuleForAllLevels(consoleTarget);

            return config;
        }
    }
}
