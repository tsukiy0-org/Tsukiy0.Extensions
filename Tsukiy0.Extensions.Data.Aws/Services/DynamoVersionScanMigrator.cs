using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

using Tsukiy0.Extensions.Data.Aws.Configs;
using Tsukiy0.Extensions.Data.Aws.Extensions;
using Tsukiy0.Extensions.Data.Services;

namespace Tsukiy0.Extensions.Data.Aws.Services
{
    public class DynamoVersionScanMigrator<T>
    {
        private readonly AbstractVersionDaoMapper<T, Dictionary<string, AttributeValue>> mapper;
        private readonly IAmazonDynamoDB client;
        private readonly DynamoVersionScanMigratorConfig config;

        public DynamoVersionScanMigrator(AbstractVersionDaoMapper<T, Dictionary<string, AttributeValue>> mapper, IAmazonDynamoDB client, DynamoVersionScanMigratorConfig config)
        {
            this.mapper = mapper;
            this.client = client;
            this.config = config;
        }

        public async Task Migrate()
        {
            await client.ScanAllAsync(
                new ScanRequest
                {
                    TableName = config.TableName,
                }
            )
                .Buffer(25)
                .SelectAwait(async items =>
                {
                    return await Task.WhenAll(items.Select(async _ =>
                    {
                        var dest = await mapper.From(_);
                        return await mapper.To(dest);
                    }));
                })
                .ForEachAwaitAsync(async _ =>
                {
                    await client.PutAll(config.TableName, _);
                });
        }
    }
}