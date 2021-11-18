using Amazon.Lambda.AspNetCoreServer;

using Tsukiy0.Extensions.Templates.Default.Api.Extensions;

namespace Tsukiy0.Extensions.Templates.Default;
public class Function : APIGatewayProxyFunction
{
    protected override void Init(IHostBuilder builder)
    {
        builder.Configure();
    }
}