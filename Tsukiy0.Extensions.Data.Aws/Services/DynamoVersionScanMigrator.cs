using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tsukiy0.Extensions.Data.Aws.Configs;
using Tsukiy0.Extensions.Data.Services;

namespace Tsukiy0.Extensions.Data.Aws.Services
{
    public class DynamoVersionScanMigrator<T>
    {
        // private readonly AbstractVersionDaoMapper<T, Dictionary<string, AttributeValue>> mapper;
        // private readonly IAmazonDynamoDB client;
        // private readonly DynamoVersionScanMigratorConfig config;

        // public DynamoScanMigrator(BaseDynamoMapper<T> mapper, IAmazonDynamoDB client, DynamoScanMigratorConfig config)
        // {
        //     this.mapper = mapper;
        //     this.client = client;
        //     this.config = config;
        // }

        // public async Task Migrate()
        // {
        //     await client.ForEachScannedPage(
        //         new ScanRequest
        //         {
        //             TableName = config.TableName,
        //             Limit = config.Limit,
        //         },
        //         async (IEnumerable<Dictionary<string, AttributeValue>> items) =>
        //         {
        //             var daos = await Task.WhenAll(items.Select(async _ =>
        //             {
        //                 var dto = await mapper.ToDto(_);
        //                 return await mapper.ToDao(dto);
        //             }));

        //             await client.BatchPut(config.TableName, daos);
        //         }
        //     );
        // }
    }
}
