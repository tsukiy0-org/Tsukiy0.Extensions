using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Tsukiy0.Extensions.Example.AspNetCore;
using Tsukiy0.Extensions.Logging.NLog.Extensions;

namespace Tsukiy0.Extensions.Example.AspNetCore.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder Configure(this IHostBuilder builder)
        {
            return builder
                .ConfigureNLogLogging("api")
                .ConfigureWebHostDefaults(_ =>
                {
                    _.UseStartup<Startup>();
                });
        }
    }
}
