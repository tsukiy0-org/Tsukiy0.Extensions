using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Tsukiy0.Extensions.Data.Aws.Extensions;
using Tsukiy0.Extensions.Data.Aws.Services;
using Tsukiy0.Extensions.Data.Services;

namespace Tsukiy0.Extensions.Data.Aws.IntegrationTests.Helpers
{
    public class TestModelVersionDaoMapper : AbstractDynamoVersionDaoMapper<TestModel>
    {
        private readonly IAmazonDynamoDB _client;
        private readonly TestConfig _config;

        public TestModelVersionDaoMapper(TestModelV1DaoMapper v1Mapper, IAmazonDynamoDB client, TestConfig config)
        : base(new List<VersionMapper<TestModel, Dictionary<string, AttributeValue>>> {
            new VersionMapper<TestModel, Dictionary<string, AttributeValue>>(1, v1Mapper)
        })
        {
            _client = client;
            _config = config;
        }

        protected override async Task OnVersionMismatch(TestModel t, int currentVersion)
        {
            await _client.PutItemAsync(new PutItemRequest
            {
                TableName = _config.TestDynamoTableName,
                Item = To(t).ToAttributeMap()
            });
        }
    }

}

