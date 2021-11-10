using System;
using System.Collections.Generic;
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

namespace Tsukiy0.Extensions.Data.Aws.IntegrationTests
{
    public class DynamoTestModelRepositoryFixture : IDisposable
    {
        public readonly DynamoTestModelRepository Sut;
        private readonly IHost _host;

        public DynamoTestModelRepositoryFixture()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(_ =>
                {
                    _.AddSsmParameterConfiguration(new List<SsmParameterMap>
                    {
                        new SsmParameterMap{
                            ParameterKey = "/tsukiy0/extensions/test-table/table-name",
                            ConfigurationKey = $"{nameof(DynamoTestModelRepositoryConfig)}:{nameof(DynamoTestModelRepositoryConfig.TableName)}"
                        }
                    });
                })
                .ConfigureServices((ctx, _) =>
                {
                    _.AddConfig<DynamoTestModelRepositoryConfig>(ctx.Configuration);
                    _.AddAWSService<IAmazonDynamoDB>();
                    _.AddScoped<TestModelV1DaoMapper>();
                    _.AddScoped<IDynamoDaoMapper<TestModel>, TestModelVersionDaoMapper>();
                    _.AddScoped<DynamoTestModelRepository>();
                })
                .Build();

            Sut = _host.Services.GetRequiredService<DynamoTestModelRepository>();
        }

        public void Dispose()
        {
            _host.Dispose();
        }
    }

}
