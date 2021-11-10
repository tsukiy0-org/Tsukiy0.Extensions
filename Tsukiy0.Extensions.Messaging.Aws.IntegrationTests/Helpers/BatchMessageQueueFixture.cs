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
using Tsukiy0.Extensions.Correlation.Services;
using System;
using Amazon.Batch;
using Tsukiy0.Extensions.Example.Core.Services;

namespace Tsukiy0.Extensions.Messaging.Aws.IntegrationTests.Helpers
{
    public class BatchMessageQueueFixture : IDisposable
    {
        public readonly BatchSaveTestModelQueue Sut;
        public readonly ITestModelRepository Repo;
        private readonly IHost _host;

        public BatchMessageQueueFixture()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(_ =>
                {
                    _.AddSsmParameterConfiguration(new List<SsmParameterMap>
                    {
                        new SsmParameterMap{
                            ParameterKey = "/tsukiy0/extensions/test-table/table-name",
                            ConfigurationKey = $"{nameof(DynamoTestModelRepositoryConfig)}:{nameof(DynamoTestModelRepositoryConfig.TableName)}"
                        },
                        new SsmParameterMap{
                            ParameterKey = "/tsukiy0/extensions/batch-processor/job-definition-arn",
                            ConfigurationKey = $"{nameof(BatchSaveTestModelQueueConfig)}:{nameof(BatchSaveTestModelQueueConfig.JobDefinitionArn)}"
                        },
                        new SsmParameterMap{
                            ParameterKey = "/tsukiy0/extensions/batch-processor/job-queue-arn",
                            ConfigurationKey = $"{nameof(BatchSaveTestModelQueueConfig)}:{nameof(BatchSaveTestModelQueueConfig.JobQueueArn)}"
                        }
                    });
                })
                .ConfigureServices((ctx, _) =>
                {
                    _.AddConfig<DynamoTestModelRepositoryConfig>(ctx.Configuration);
                    _.AddAWSService<IAmazonDynamoDB>();
                    _.AddAWSService<IAmazonBatch>();

                    _.AddScoped<TestModelV1DaoMapper>();
                    _.AddScoped<IDynamoDaoMapper<TestModel>, TestModelVersionDaoMapper>();
                    _.AddScoped<ITestModelRepository, DynamoTestModelRepository>();
                    _.AddScoped<ICorrelationService>(_ => new StaticCorrelationService(Guid.NewGuid(), Guid.NewGuid()));

                    _.AddConfig<BatchSaveTestModelQueueConfig>(ctx.Configuration);
                    _.AddScoped<BatchSaveTestModelQueue>();
                })
                .Build();

            Repo = _host.Services.GetRequiredService<ITestModelRepository>();
            Sut = _host.Services.GetRequiredService<BatchSaveTestModelQueue>();
        }

        public void Dispose()
        {
            _host.Dispose();
        }
    }
}
