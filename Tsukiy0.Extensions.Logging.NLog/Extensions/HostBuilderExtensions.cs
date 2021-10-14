using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Hosting;

namespace Tsukiy0.Extensions.Logging.NLog.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddLoggingExtensions(this IHostBuilder builder, string name)
        {
            var config = new LoggingConfiguration();
            config.ConfigureNoMicrosoftLogs();
            config.ConfigureConsole(name);
            LogManager.Configuration = config;

            return builder
                .ConfigureLogging((_, logging) =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                })
                .UseNLog();
        }
    }
}
