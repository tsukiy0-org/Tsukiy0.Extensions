using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tsukiy0.Extensions.Data.Aws.Services;
using Tsukiy0.Extensions.Configuration.Aws.Extensions;
using Tsukiy0.Extensions.Configuration.Aws.Models;
using Tsukiy0.Extensions.Configuration.Extensions;
using Tsukiy0.Extensions.Example.Infrastructure.Services;
using Tsukiy0.Extensions.Example.Core.Models;
using Tsukiy0.Extensions.Example.Infrastructure.Configs;
using Amazon.SQS;
using Tsukiy0.Extensions.Correlation.Services;
using System;
using Amazon.Batch;
using Tsukiy0.Extensions.Example.Core.Services;

namespace Tsukiy0.Extensions.Messaging.Aws.IntegrationTests.Helpers
{
    public class SqsMessageQueueFixture : IDisposable
    {
        public readonly SqsSaveTestModelQueue Sut;
        public readonly ITestModelRepository Repo;
        private readonly IHost _host;

        public SqsMessageQueueFixture()
        {
            _host = Host.CreateDefaultBuilder()
               .ConfigureAppConfiguration(_ =>
               {
                   _.AddSsmParameterConfiguration(new List<SsmParameterMap>
                   {
                        new SsmParameterMap("/tsukiy0/extensions/test-table/table-name", $"{nameof(DynamoTestModelRepositoryConfig)}:{nameof(DynamoTestModelRepositoryConfig.TableName)}"),
                        new SsmParameterMap("/tsukiy0/extensions/sqs-processor/queue-url", $"{nameof(SqsSaveTestModelQueueConfig)}:{nameof(SqsSaveTestModelQueueConfig.QueueUrl)}"),
                   });
               })
               .ConfigureServices((ctx, _) =>
               {
                   _.AddAWSService<IAmazonDynamoDB>();
                   _.AddAWSService<IAmazonSQS>();

                   _.AddScoped<TestModelV1DaoMapper>();
                   _.AddScoped<IDynamoDaoMapper<TestModel>, TestModelVersionDaoMapper>();
                   _.AddConfig<DynamoTestModelRepositoryConfig>(ctx.Configuration);
                   _.AddScoped<ITestModelRepository, DynamoTestModelRepository>();
                   _.AddScoped<ICorrelationService>(_ => new StaticCorrelationService(Guid.NewGuid(), Guid.NewGuid()));

                   _.AddConfig<SqsSaveTestModelQueueConfig>(ctx.Configuration);
                   _.AddScoped<SqsSaveTestModelQueue>();
               })
               .Build();

            Repo = _host.Services.GetRequiredService<ITestModelRepository>();
            Sut = _host.Services.GetRequiredService<SqsSaveTestModelQueue>();
        }

        public void Dispose()
        {
            _host.Dispose();
        }
    }
}
