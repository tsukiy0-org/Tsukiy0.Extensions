using Tsukiy0.Extensions.Logging.NLog.Extensions;

namespace Tsukiy0.Extensions.Templates.Default.Api.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder Configure(this IHostBuilder builder)
    {
        return builder
            .ConfigureAppConfiguration(_ => { })
            .ConfigureNLogLogging("api")
            .ConfigureWebHostDefaults(_ =>
            {
                _.UseStartup<Startup>();
            });
    }
}