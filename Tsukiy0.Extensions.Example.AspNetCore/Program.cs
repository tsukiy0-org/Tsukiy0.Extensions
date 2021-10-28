using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Tsukiy0.Extensions.Example.AspNetCore.Extensions;
using Tsukiy0.Extensions.Logging.NLog.Extensions;

namespace Tsukiy0.Extensions.Example.AspNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .Configure();
    }
}
