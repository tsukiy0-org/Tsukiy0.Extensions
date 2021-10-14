using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Layouts;
using NLog.Targets;
using Tsukiy0.Extensions.NLog.Renderers;

namespace Tsukiy0.Extensions.Logging.NLog.Extensions
{
    public static class LoggingConfigurationExtensions
    {
        public static void ConfigureNoMicrosoftLogs(this LoggingConfiguration config)
        {
            var target = new NullTarget();
            config.AddRuleForAllLevels(target, "Microsoft.*", true);
            config.AddRuleForAllLevels(target, "System.*", true);
        }

        public static void ConfigureConsole(this LoggingConfiguration config, string application)
        {
            var renderer = new LogLayoutRenderer(application);

            LayoutRenderer.Register("2bb7d2b55dbadfa6466c951a1606460d", _ =>
            {
                return renderer.Render(_);
            });

            var consoleTarget = new ConsoleTarget()
            {
                Layout = Layout.FromString("${2bb7d2b55dbadfa6466c951a1606460d}")
            };

            config.AddRuleForAllLevels(consoleTarget);
        }
    }
}
