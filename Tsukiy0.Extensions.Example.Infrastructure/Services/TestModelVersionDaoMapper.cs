using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

using Tsukiy0.Extensions.Data.Aws.Extensions;
using Tsukiy0.Extensions.Data.Aws.Services;
using Tsukiy0.Extensions.Data.Services;
using Tsukiy0.Extensions.Example.Core.Models;
using Tsukiy0.Extensions.Example.Infrastructure.Configs;

namespace Tsukiy0.Extensions.Example.Infrastructure.Services
{
    public class TestModelVersionDaoMapper : AbstractDynamoVersionDaoMapper<TestModel>
    {
        private readonly IAmazonDynamoDB _client;
        private readonly DynamoTestModelRepositoryConfig _config;

        public TestModelVersionDaoMapper(TestModelV1DaoMapper v1Mapper, IAmazonDynamoDB client, DynamoTestModelRepositoryConfig config)
        : base(new List<VersionMapper<TestModel, Dictionary<string, AttributeValue>>> {
            new VersionMapper<TestModel, Dictionary<string, AttributeValue>>{
                Version = 1,
                Mapper = v1Mapper
            }
        })
        {
            _client = client;
            _config = config;
        }

        protected override async Task OnVersionMismatch(TestModel t, int currentVersion)
        {
            await _client.PutItemAsync(new PutItemRequest
            {
                TableName = _config.TableName,
                Item = To(t).ToAttributeMap()
            });
        }
    }

}