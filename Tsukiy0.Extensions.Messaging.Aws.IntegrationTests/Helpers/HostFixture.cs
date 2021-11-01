using System;
using Microsoft.Extensions.Hosting;

namespace Tsukiy0.Extensions.Messaging.Aws.IntegrationTests.Helpers
{
    public class HostFixture : IDisposable
    {
        public readonly IHost Host;

        public HostFixture()
        {
            Host = TestHostBuilder.Build();
        }


        public void Dispose()
        {
            Host.Dispose();
        }
    }
}
