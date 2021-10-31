using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tsukiy0.Extensions.Data.Aws.Extensions;
using Tsukiy0.Extensions.Data.Aws.IntegrationTests.Helpers;
using Tsukiy0.Extensions.Data.Aws.Models;
using Tsukiy0.Extensions.Data.Aws.Services;
using Xunit;

namespace Tsukiy0.Extensions.Data.Aws.IntegrationTests.Extensions
{
    public partial class DynamoExtensionsTests
    {
        private readonly IAmazonDynamoDB _sut;
        private readonly IDynamoDaoMapper<TestModel> _mapper;
        private readonly TestConfig _config;

        public DynamoExtensionsTests()
        {
            var host = TestHostBuilder.Build();
            _sut = host.Services.GetRequiredService<IAmazonDynamoDB>();
            _mapper = host.Services.GetRequiredService<IDynamoDaoMapper<TestModel>>();
            _config = host.Services.GetRequiredService<TestConfig>();
        }

        [Fact]
        public async void PutAll()
        {
            // Arrange
            var ns = Guid.NewGuid();
            var models = new Faker<TestModel>()
                .CustomInstantiator(f => new TestModel(
                    Id: Guid.NewGuid(),
                    Namespace: ns
                ))
                .GenerateForever().Take(20);
            var items = await Task.WhenAll(models.Select(async _ => await _mapper.To(_)));

            // Act
            await _sut.PutAll(_config.TestDynamoTableName, items);
            var actual = await _sut.QueryAllAsync(new QueryRequest
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
                }
            }).ToListAsync();

            // Assert
            actual.Should().HaveCount(models.Count());
        }
    }
}
