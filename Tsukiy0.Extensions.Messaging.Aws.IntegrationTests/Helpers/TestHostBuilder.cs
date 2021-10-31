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

namespace Tsukiy0.Extensions.Messaging.Aws.IntegrationTests.Helpers
{
    public static class TestHostBuilder
    {
        public static IHost Build()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(_ =>
                {
                    _.AddSsmParameterConfiguration(new List<SsmParameterMap>
                    {
                        new SsmParameterMap("/tsukiy0/extensions/test-table/table-name", $"{nameof(DynamoTestModelRepositoryConfig)}:{nameof(DynamoTestModelRepositoryConfig.TableName)}"),
                        new SsmParameterMap("/tsukiy0/extensions/sqs-processor/queue-url", $"{nameof(SqsSaveTestModelQueueConfig)}:{nameof(SqsSaveTestModelQueueConfig.QueueUrl)}")
                    });
                })
                .ConfigureServices((ctx, _) =>
                {
                    _.AddConfig<DynamoTestModelRepositoryConfig>(ctx.Configuration);
                    _.AddAWSService<IAmazonDynamoDB>();
                    _.AddAWSService<IAmazonSQS>();

                    _.AddScoped<TestModelV1DaoMapper>();
                    _.AddScoped<IDynamoDaoMapper<TestModel>, TestModelVersionDaoMapper>();
                    _.AddScoped<DynamoTestModelRepository>();

                    _.AddConfig<SqsSaveTestModelQueueConfig>(ctx.Configuration);
                    _.AddScoped<SqsSaveTestModelQueue>();
                })
                .Build();
        }
    }
}
