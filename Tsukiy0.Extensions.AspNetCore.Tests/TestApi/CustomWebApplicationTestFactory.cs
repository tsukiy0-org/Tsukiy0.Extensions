using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Tsukiy0.Extensions.AspNetCore.Tests.TestApi
{
    public class CustomWebApplicationFactory
        : WebApplicationFactory<Startup>
    {
        public static string ApiKey = Guid.NewGuid().ToString();

        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.Sources.Clear();
                    config
                        .AddInMemoryCollection(new Dictionary<string, string>
                        {
                            ["ApiKeyAuthConfig:ApiKeys:Service"] = ApiKey
                        });
                })
                .ConfigureWebHostDefaults(_ =>
                {
                    _.UseStartup<Startup>().UseTestServer();
                });
        }
    }
}