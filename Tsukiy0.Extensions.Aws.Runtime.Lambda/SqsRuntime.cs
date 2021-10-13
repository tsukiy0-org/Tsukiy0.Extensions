using System;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.SQSEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Tsukiy0.Extensions.Aws.Core.Models;
using Tsukiy0.Extensions.Correlation.Services;
using Tsukiy0.Extensions.Logging.Core.Extensions;
using Tsukiy0.Extensions.Logging.Nlog.Extensions;

namespace Tsukiy0.Extensions.Aws.Runtime.Lambda
{
    public abstract class SqsRuntime<T>
    {
        protected abstract void ConfigureServices(HostBuilderContext ctx, IServiceCollection services);
        protected abstract Task Run(IHost host, T message);

        private IHost CreateHost(SendMessageEnvelope<T> message)
        {
            return Host.CreateDefaultBuilder()
                .AddLoggingExtensions("SqsRuntime")
                .ConfigureServices((ctx, services) =>
                {
                    services.AddLogging(l =>
                    {
                        l.ClearProviders();
                        l.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                        l.AddNLog();
                    });
                    services.AddScoped(_ => message);
                    services.AddScoped<ICorrelationService, SqsCorrelationService<T>>();

                    ConfigureServices(ctx, services);
                })
                .Build();
        }

        public async Task FunctionHandler(SQSEvent input)
        {
            var message = JsonSerializer.Deserialize<SendMessageEnvelope<T>>(input.Records[0].Body);
            var host = CreateHost(message);
            var logger = host.Services.GetRequiredService<ILogger<SqsRuntime<T>>>();
            var correlationService = host.Services.GetRequiredService<ICorrelationService>();

            await logger.WithCorrelation(correlationService, async () =>
            {
                try
                {
                    logger.LogInformation("New message: {message}", message);
                    await Run(host, message.Message);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Failed to process message");
                }
            });
        }
    }
}
