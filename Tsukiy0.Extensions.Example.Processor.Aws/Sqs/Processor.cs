using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Tsukiy0.Extensions.Logging.NLog.Extensions;
using Tsukiy0.Extensions.Processor.Services;

namespace Tsukiy0.Extensions.Example.Processor.Aws.Sqs
{
    public class Processor : DefaultProcessor<Guid>
    {
        protected override IHostBuilder ConfigureHost(IHostBuilder builder)
        {
            return builder
                .ConfigureNLogLogging(nameof(Processor))
                .ConfigureServices((ctx, _) =>
                {
                });
        }

        protected override Task Run(IHost host, Guid body)
        {
            throw new NotImplementedException();
        }
    }

}
