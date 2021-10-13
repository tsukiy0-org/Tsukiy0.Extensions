using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.SQSEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tsukiy0.Extensions.Aws.Core.Models;
using Tsukiy0.Extensions.Aws.Core.Services;
using Tsukiy0.Extensions.Aws.Infrastructure.Services;
using Tsukiy0.Extensions.Logging.Core.Extensions;
using Tsukiy0.Extensions.Logging.Core.Services;
using Tsukiy0.Extensions.Logging.Nlog.Extensions;

namespace Tsukiy0.Extensions.Aws.Runtime.Lambda
{
    public abstract class BatchSqsRuntime<T>
    {
        protected abstract void ConfigureServices(HostBuilderContext ctx, IServiceCollection services);
        protected abstract Task Run(IHost host, T message);

        private IHost CreateHost(SendMessageEnvelope<T> message)
        {
            return Host.CreateDefaultBuilder()
                .AddLoggingExtensions("BatchSqsRuntime")
                .ConfigureServices((ctx, services) =>
                {
                    services.AddScoped(_ => message);
                    services.AddScoped<ICorrelationService, SqsCorrelationService<T>>();
                    services.AddScoped<ISqsClient<T>, SqsClient<T>>();

                    ConfigureServices(ctx, services);
                })
                .Build();
        }

        public async Task FunctionHandler(SQSEvent input)
        {
            var tasks = input.Records.Select(async _ =>
            {
                var message = JsonSerializer.Deserialize<SendMessageEnvelope<T>>(_.Body);
                var host = CreateHost(message);
                var logger = host.Services.GetRequiredService<ILogger<SqsRuntime<T>>>();
                var sqsClient = host.Services.GetRequiredService<ISqsClient<T>>();
                var correlationService = host.Services.GetRequiredService<ICorrelationService>();

                await logger.WithCorrelation(correlationService, async () =>
                {
                    try
                    {
                        logger.LogInformation("New message: {message}", _);
                        await Run(host, message.Message);
                        await sqsClient.Delete(_.ReceiptHandle);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Failed to process message");
                    }
                });
            });

            await Task.WhenAll(tasks);
        }
    }
}
