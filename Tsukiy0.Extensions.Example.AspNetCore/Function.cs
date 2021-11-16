using Amazon.Lambda.AspNetCoreServer;

using Microsoft.Extensions.Hosting;

using Tsukiy0.Extensions.Example.AspNetCore.Extensions;

namespace Tsukiy0.Extensions.Example.AspNetCore
{
    public class Function : APIGatewayProxyFunction
    {
        protected override void Init(IHostBuilder builder)
        {
            builder.Configure();
        }
    }
}