using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tsukiy0.Extensions.Configuration.Aws.Extensions;
using Tsukiy0.Extensions.Configuration.Aws.Models;
using Tsukiy0.Extensions.Configuration.Extensions;
using Tsukiy0.Extensions.Data.Aws.Services;
using Tsukiy0.Extensions.Example.Core.Handlers;
using Tsukiy0.Extensions.Example.Core.Models;
using Tsukiy0.Extensions.Example.Infrastructure.Configs;
using Tsukiy0.Extensions.Logging.NLog.Extensions;
using Tsukiy0.Extensions.MediatR.Extensions;
using Tsukiy0.Extensions.Processor.Services;

namespace Tsukiy0.Extensions.Example.Infrastructure.Services
{
    public class SaveTestModelProcessor : DefaultProcessor<SaveTestModelRequest>
    {
        protected override IHostBuilder ConfigureHost(IHostBuilder builder)
        {
            return builder
                .ConfigureNLogLogging(nameof(SaveTestModelProcessor))
                .ConfigureAppConfiguration(_ =>
                {
                    _.AddSsmParameterConfiguration(new List<SsmParameterMap>
                    {
                        new SsmParameterMap("/tsukiy0/extensions/test-table/table-name", $"{nameof(DynamoTestModelRepositoryConfig)}:{nameof(DynamoTestModelRepositoryConfig.TableName)}")
                    });
                })
                .ConfigureServices((ctx, _) =>
                {
                    _.AddConfig<DynamoTestModelRepositoryConfig>(ctx.Configuration);
                    _.AddAWSService<IAmazonDynamoDB>();
                    _.AddScoped<TestModelV1DaoMapper>();
                    _.AddScoped<IDynamoDaoMapper<TestModel>, TestModelVersionDaoMapper>();
                    _.AddScoped<DynamoTestModelRepository>();

                    _.AddMediatR();
                    _.AddScoped<IRequestHandler<SaveTestModelRequest, Unit>, SaveTestModelHandler>();
                });
        }

        protected override async Task Run(IHost host, SaveTestModelRequest body)
        {
            var mediator = host.Services.GetRequiredService<Mediator>();
            await mediator.Send(body);
        }
    }

}
