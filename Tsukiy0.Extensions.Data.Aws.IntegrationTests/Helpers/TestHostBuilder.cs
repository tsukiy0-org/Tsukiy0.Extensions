using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tsukiy0.Extensions.Configuration.Aws.Extensions;
using Tsukiy0.Extensions.Configuration.Aws.Models;
using Tsukiy0.Extensions.Configuration.Extensions;

namespace Tsukiy0.Extensions.Data.Aws.IntegrationTests.Helpers
{
    public static class HostBuilderExtensions
    {
        public static IHost Build()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(_ =>
                {
                    _.AddSsmParameterConfiguration(new List<SsmParameterMap>
                    {
                        new SsmParameterMap("/tsukiy0/extensions/test-dynamo-table/table-name", $"{nameof(TestConfig)}:{nameof(TestConfig.TestDynamoTableName)}")
                    });
                })
                .ConfigureServices((ctx, _) =>
                {
                    _.AddConfig<TestConfig>(ctx.Configuration);

                    _.AddAWSService<IAmazonDynamoDB>();
                })
                .Build();
        }
    }
}
