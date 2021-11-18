using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

using FluentAssertions;
using FluentAssertions.Extensions;

using Moq;

using Tsukiy0.Extensions.Data.Aws.Extensions;
using Tsukiy0.Extensions.Data.Aws.Models;
using Tsukiy0.Extensions.Testing.Extensions;

using Xunit;

namespace Tsukiy0.Extensions.Data.Aws.Tests.Extensions
{
    public class DynamoExtensionsTests
    {

        [Fact]
        public void Hash()
        {
            var actual = DynamoExtensions.Hash("p1", "p2", Guid.Parse("0bda3731-5e11-4d9c-ab33-688ffb798bac"));

            actual.Should().Be("p1//p2//0bda3731-5e11-4d9c-ab33-688ffb798bac");
        }

        [Fact]
        public void ToAndFromAttributeMap()
        {
            var obj = new SerializationTest
            {
                __PK = "PK",
                PK = "PK",
                __SK = "SK",
                __Updated = DateTimeOffset.MaxValue,
                Integer = 1,
                Decimal = 1.111111M,
                String = "String",
                Boolean = true,
                List = new List<int> { 1, 2, 3 },
                Dictionary = new Dictionary<string, string>
                {
                    ["Key"] = "Value"
                },
                Object = new SerializationTest.Complex
                {
                    IsComplex = true
                },
                Date = DateTimeOffset.MaxValue,
                Guid = Guid.NewGuid(),
                Enum = SerializationTest.Enummm.CASE_2
            };
            var am = new Dictionary<string, AttributeValue>
            {
                ["__PK"] = new AttributeValue { S = "PK" },
                ["pk"] = new AttributeValue { S = "PK" },
                ["__SK"] = new AttributeValue { S = "SK" },
                ["__Updated"] = new AttributeValue { S = obj.__Updated.ToString("o") },
                ["integer"] = new AttributeValue { N = "1" },
                ["decimal"] = new AttributeValue { N = "1.111111" },
                ["string"] = new AttributeValue { S = "String" },
                ["boolean"] = new AttributeValue { BOOL = true },
                ["list"] = new AttributeValue
                {
                    L = new List<AttributeValue> {
                        new AttributeValue { N = "1" },
                        new AttributeValue { N = "2" },
                        new AttributeValue { N = "3" },
                    }
                },
                ["dictionary"] = new AttributeValue
                {
                    M = new Dictionary<string, AttributeValue>
                    {

                        ["Key"] = new AttributeValue { S = "Value" }
                    }
                },
                ["object"] = new AttributeValue
                {
                    M = new Dictionary<string, AttributeValue>
                    {

                        ["isComplex"] = new AttributeValue { BOOL = true }
                    }
                },
                ["date"] = new AttributeValue { S = obj.Date.ToString("o") },
                ["guid"] = new AttributeValue { S = obj.Guid.ToString() },
                ["enum"] = new AttributeValue { S = "CASE_2" }
            };

            var toActual = obj.ToAttributeMap();
            var fromActual = toActual.FromAttributeMap<SerializationTest>();

            toActual.Should().BeEquivalentTo(am, o => o.ComparingDateTimesCloseTo(2.Seconds()));
            fromActual.Should().BeEquivalentTo(obj, o => o.ComparingDateTimesCloseTo(2.Seconds()));
        }

        [Fact]
        public async void ScanAllAsync()
        {
            // Arrange
            var page1Request = new ScanRequest
            {
                ExclusiveStartKey = null
            };
            var page1Response = new ScanResponse
            {
                Items = new List<Dictionary<string, AttributeValue>> {
                    new Point(1, 2).ToAttributeMap(),
                    new Point(3, 4).ToAttributeMap(),
                },
                LastEvaluatedKey = new DynamoKey
                {
                    PK = "A",
                    SK = "B"
                }.ToAttributeMap()
            };

            var page2Request = new ScanRequest
            {
                ExclusiveStartKey = page1Response.LastEvaluatedKey
            };
            var page2Response = new ScanResponse
            {
                Items = new List<Dictionary<string, AttributeValue>> {
                    new Point(5, 6).ToAttributeMap(),
                    new Point(7, 8).ToAttributeMap(),
                },
                LastEvaluatedKey = null
            };

            var mockClient = new Mock<IAmazonDynamoDB>();
            var requests = new List<ScanRequest>();
            mockClient.SetupSequence(_ => _.ScanAsync(Capture.In(requests), CancellationToken.None))
                    .ReturnsAsync(page1Response)
                    .ReturnsAsync(page2Response);

            // Act
            var actual = await mockClient.Object.ScanAllAsync(page1Request)
                .Select(_ => _.FromAttributeMap<Point>())
                .ToListAsync();

            // Assert
            actual.Should().BeEquivalentTo(new List<Point> {
                new Point(1, 2),
                new Point(3, 4),
                new Point(5, 6),
                new Point(7, 8),
            });
            requests.Should().BeEquivalentTo(new List<ScanRequest>
            {
                page1Request,
                page2Request
});
        }

        [Fact]
        public async void QueryAllAsync()
        {
            // Arrange
            var page1Request = new QueryRequest
            {
                ExclusiveStartKey = null
            };
            var page1Response = new QueryResponse
            {
                Items = new List<Dictionary<string, AttributeValue>> {
                    new Point(1, 2).ToAttributeMap(),
                    new Point(3, 4).ToAttributeMap(),
                },
                LastEvaluatedKey = new DynamoKey
                {
                    PK = "A",
                    SK = "B"
                }.ToAttributeMap()
            };

            var page2Request = new QueryRequest
            {
                ExclusiveStartKey = page1Response.LastEvaluatedKey
            };
            var page2Response = new QueryResponse
            {
                Items = new List<Dictionary<string, AttributeValue>> {
                    new Point(5, 6).ToAttributeMap(),
                    new Point(7, 8).ToAttributeMap(),
                },
                LastEvaluatedKey = null
            };

            var mockClient = new Mock<IAmazonDynamoDB>();
            var requests = new List<QueryRequest>();
            mockClient.SetupSequence(_ => _.QueryAsync(Capture.In(requests), CancellationToken.None))
                .ReturnsAsync(page1Response)
                .ReturnsAsync(page2Response);

            // Act
            var actual = await mockClient.Object.QueryAllAsync(page1Request)
                .Select(_ => _.FromAttributeMap<Point>())
                .ToListAsync();

            // Assert
            actual.Should().BeEquivalentTo(new List<Point> {
                new Point(1, 2),
                new Point(3, 4),
                new Point(5, 6),
                new Point(7, 8),
            });
            requests.Should().BeEquivalentTo(new List<QueryRequest>
            {
                page1Request,
                page2Request
            });
        }

        [Fact]
        public async void PutAll()
        {
            // Arrange
            var tableName = "some-table";
            var items = new List<int>(new int[60]).Select((_, i) => new DynamoKey
            {
                PK = $"{i}",
                SK = $"{i + 1}"
            }.ToAttributeMap());
            var mockClient = new Mock<IAmazonDynamoDB>();
            var requests = new List<BatchWriteItemRequest>();
            mockClient.Setup(_ => _.BatchWriteItemAsync(Capture.In(requests), CancellationToken.None));

            // Act
            await mockClient.Object.PutAll(tableName, items);

            // Assert
            requests.Should().HaveCount(3);
            requests[0].RequestItems[tableName].Should().HaveCount(25);
            requests[1].RequestItems[tableName].Should().HaveCount(25);
            requests[2].RequestItems[tableName].Should().HaveCount(10);
        }

        [Fact]
        public async void DeleteAll()
        {
            // Arrange
            var tableName = "some-table";
            var items = new List<int>(new int[60]).Select((_, i) => new DynamoKey
            {
                PK = $"{i}",
                SK = $"{i + 1}"
            }.ToAttributeMap());
            var mockClient = new Mock<IAmazonDynamoDB>();
            var requests = new List<BatchWriteItemRequest>();
            mockClient.Setup(_ => _.BatchWriteItemAsync(Capture.In(requests), CancellationToken.None));

            // Act
            await mockClient.Object.DeleteAll(tableName, items);

            // Assert
            requests.Should().HaveCount(3);
            requests[0].RequestItems[tableName].Should().HaveCount(25);
            requests[1].RequestItems[tableName].Should().HaveCount(25);
            requests[2].RequestItems[tableName].Should().HaveCount(10);
        }

        private class SerializationTest
        {
            public string __PK { get; init; }
            public string PK { get; init; }
            public string __SK { get; init; }
            public DateTimeOffset __Updated { get; init; }
            public int Integer { get; init; }
            public decimal Decimal { get; init; }
            public string String { get; init; }
            public bool Boolean { get; init; }
            public IEnumerable<int> List { get; init; }
            public IDictionary<string, string> Dictionary { get; init; }
            public Complex Object { get; init; }
            public DateTimeOffset Date { get; init; }
            public Guid Guid { get; init; }
            public Enummm Enum { get; init; }

            public class Complex
            {
                public bool IsComplex { get; init; }
            }

            public enum Enummm
            {
                CASE_1,
                CASE_2
            }
        }
    }
}