using System.Collections.Generic;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Tsukiy0.Extensions.Configuration.Aws.Extensions;
using Tsukiy0.Extensions.Configuration.Aws.Models;
using Tsukiy0.Extensions.Logging.NLog.Extensions;

namespace Tsukiy0.Extensions.Example.AspNetCore.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder Configure(this IHostBuilder builder)
        {
            return builder
                .ConfigureAppConfiguration(_ =>
                {
                    _.AddSsmParameterConfiguration(new List<SsmParameterMap> {
                        new SsmParameterMap{
                            ParameterKey = "/tsukiy0/extensions/aspnetcore/apikey/service",
                            ConfigurationKey = "ApiKeyAuthConfig:ApiKeys:Service"
                        }
                    });
                })
                .ConfigureNLogLogging("api")
                .ConfigureWebHostDefaults(_ =>
                {
                    _.UseStartup<Startup>();
                });
        }
    }
}