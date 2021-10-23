using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tsukiy0.Extensions.Correlation.Services;
using Tsukiy0.Extensions.Logging.Extensions;
using Tsukiy0.Extensions.Messaging.Models;

namespace Tsukiy0.Extensions.Processor.Services
{
    public abstract class DefaultProcessor<T> : IProcessor<T>
    {
        protected abstract IHostBuilder ConfigureHost(IHostBuilder builder);
        protected abstract Task Run(IHost host, T body);

        private IHost CreateHost(Message<T> message)
        {
            var hostBuilder = ConfigureHost(
                Host.CreateDefaultBuilder()
                    .ConfigureServices(s =>
                    {
                        s.AddScoped<ICorrelationService>(_ =>
                        {
                            return new MessageCorrelationService(message.Header);
                        });
                    })
            );

            return hostBuilder.Build();
        }
        public async Task Run(Message<T> message)
        {
            var host = CreateHost(message);
            var logger = host.Services.GetRequiredService<ILogger<DefaultProcessor<T>>>();
            var correlationService = host.Services.GetRequiredService<ICorrelationService>();

            await logger.WithCorrelation(correlationService, async () =>
            {
                try
                {
                    await Run(host, message.Body);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Transaction failed");
                }
                finally
                {
                    logger.LogInformation("Transaction {message}", message);
                }
            });
        }
    }
}
