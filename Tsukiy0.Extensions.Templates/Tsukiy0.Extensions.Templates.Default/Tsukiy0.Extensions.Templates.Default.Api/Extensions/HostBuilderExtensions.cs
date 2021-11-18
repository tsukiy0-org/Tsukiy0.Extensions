using Tsukiy0.Extensions.Logging.NLog.Extensions;

namespace Tsukiy0.Extensions.Templates.Default.Api.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder Configure(this IHostBuilder builder)
    {
        return builder
            .ConfigureAppConfiguration(_ => { })
            .ConfigureNLogLogging(typeof(Startup).Namespace)
            .ConfigureWebHostDefaults(_ =>
            {
                _.UseStartup<Startup>();
            });
    }
}