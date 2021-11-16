using System;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog;
using NLog.Config;
using NLog.Extensions.Hosting;

namespace Tsukiy0.Extensions.Logging.NLog.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureNLogLogging(this IHostBuilder builder, string name, Action<LoggingConfiguration> configureNLog)
        {
            var config = new LoggingConfiguration();
            config.ConfigureNoMicrosoftLogs();
            config.ConfigureConsole(name);
            configureNLog(config);
            LogManager.Configuration = config;

            return builder
                .ConfigureLogging((_, logging) =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                })
                .UseNLog();
        }

        public static IHostBuilder ConfigureNLogLogging(this IHostBuilder builder, string name)
        {
            return builder.ConfigureNLogLogging(name, (_) => { });
        }
    }
}