using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Tsukiy0.Extensions.Data.Aws.Extensions;
using Tsukiy0.Extensions.Data.Aws.Models;
using Tsukiy0.Extensions.Data.Aws.Services;

namespace Tsukiy0.Extensions.Data.Aws.IntegrationTests.Helpers
{
    public class TestModelDynamoRepository
    {
        private readonly IAmazonDynamoDB _client;
        private readonly TestConfig _config;
        private readonly IDynamoDaoMapper<TestModel> _mapper;

        public TestModelDynamoRepository(IAmazonDynamoDB client, TestConfig config, IDynamoDaoMapper<TestModel> mapper)
        {
            _client = client;
            _config = config;
            _mapper = mapper;
        }

        public async Task PutAll(IEnumerable<TestModel> models)
        {
            var items = await Task.WhenAll(models.Select(async _ => await _mapper.To(_)));
            await _client.PutAll(_config.TestDynamoTableName, items);
        }

        public async Task DeleteAll(IEnumerable<TestModel> models)
        {
            var keys = models.Select(_ => new DynamoKey(_.Namespace.ToString(), _.Id.ToString()).ToPrimaryKey().ToAttributeMap());
            await _client.DeleteAll(_config.TestDynamoTableName, keys);
        }

        public async Task<IEnumerable<TestModel>> ListByNamespace(Guid ns)
        {
            return await _client.QueryAllAsync(new QueryRequest
            {
                TableName = _config.TestDynamoTableName,
                KeyConditionExpression = "#PK = :PK",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    ["#PK"] = nameof(IDynamoPrimaryKey.__PK)
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    [":PK"] = new AttributeValue { S = ns.ToString() }
                },
                Limit = 10
            })
            .SelectAwait(async _ => await _mapper.From(_))
            .ToListAsync();
        }
    }

}
