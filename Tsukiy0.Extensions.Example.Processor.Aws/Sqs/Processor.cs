using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tsukiy0.Extensions.Configuration.Aws.Extensions;
using Tsukiy0.Extensions.Configuration.Aws.Models;
using Tsukiy0.Extensions.Configuration.Extensions;
using Tsukiy0.Extensions.Data.Aws.Services;
using Tsukiy0.Extensions.Example.Core.Models;
using Tsukiy0.Extensions.Example.Infrastructure.Configs;
using Tsukiy0.Extensions.Example.Infrastructure.Services;
using Tsukiy0.Extensions.Logging.NLog.Extensions;
using Tsukiy0.Extensions.Processor.Services;

namespace Tsukiy0.Extensions.Example.Processor.Aws.Sqs
{
    public class Processor : DefaultProcessor<TestModel>
    {
        protected override IHostBuilder ConfigureHost(IHostBuilder builder)
        {
            return builder
                .ConfigureNLogLogging(typeof(Processor).Namespace)
                .ConfigureAppConfiguration(_ =>
                {
                    _.AddSsmParameterConfiguration(new List<SsmParameterMap>
                    {
                        new SsmParameterMap("/tsukiy0/extensions/test-table/table-name", $"{nameof(TestModelDynamoRepositoryConfig)}:{nameof(TestModelDynamoRepositoryConfig.TableName)}")
                    });
                })
                .ConfigureServices((ctx, _) =>
                {
                    _.AddConfig<TestModelDynamoRepositoryConfig>(ctx.Configuration);
                    _.AddAWSService<IAmazonDynamoDB>();
                    _.AddScoped<TestModelV1DaoMapper>();
                    _.AddScoped<IDynamoDaoMapper<TestModel>, TestModelVersionDaoMapper>();
                    _.AddScoped<DynamoTestModelRepository>();
                });
        }

        protected override async Task Run(IHost host, TestModel body)
        {
            var repo = host.Services.GetRequiredService<DynamoTestModelRepository>();
            await repo.PutAll(new List<TestModel> { body });
        }
    }

}
