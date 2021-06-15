using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Tsukiy0.Extensions.Logging.Nlog.Extensions;

namespace Tsukiy0.Extensions.TestBed.AspNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .AddLoggingExtensions("Tsukiy0.Extensions.TestBed.AspNetCore");
    }
}
