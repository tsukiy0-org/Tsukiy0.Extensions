using Tsukiy0.Extensions.Templates.Default.Api.Extensions;

namespace Tsukiy0.Extensions.Templates.Default.Api;

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