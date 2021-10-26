using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Tsukiy0.Extensions.Core.Extensions;
using Tsukiy0.Extensions.Json.Extensions;

namespace Tsukiy0.Extensions.Data.Aws.Extensions
{
    public static class DynamoExtensions
    {
        public static string Hash(params object[] parts)
        {
            return string.Join("//", parts);
        }

        public static Dictionary<string, AttributeValue> ToAttributeMap(this object input)
        {
            return Document.FromJson(JsonSerializer.Serialize(input, JsonSerializerExtensions.DefaultOptions)).ToAttributeMap();
        }

        public static T FromAttributeMap<T>(this Dictionary<string, AttributeValue> input)
        {
            return JsonSerializer.Deserialize<T>(Document.FromAttributeMap(input).ToJson(), JsonSerializerExtensions.DefaultOptions);
        }

        public static async IAsyncEnumerable<Dictionary<string, AttributeValue>> QueryAllAsync(this IAmazonDynamoDB client, QueryRequest request)
        {
            Dictionary<string, AttributeValue> lastEvaluatedKey = request.ExclusiveStartKey;

            do
            {
                request.ExclusiveStartKey = lastEvaluatedKey;
                var response = await client.QueryAsync(request);
                foreach (var item in response.Items)
                {
                    yield return item;
                }

                lastEvaluatedKey = response.LastEvaluatedKey;
            } while (lastEvaluatedKey is not null && lastEvaluatedKey.Count != 0);
        }

        public static async IAsyncEnumerable<Dictionary<string, AttributeValue>> ScanAllAsync(this IAmazonDynamoDB client, ScanRequest request)
        {
            Dictionary<string, AttributeValue> lastEvaluatedKey = request.ExclusiveStartKey;

            do
            {
                request.ExclusiveStartKey = lastEvaluatedKey;
                var response = await client.ScanAsync(request);
                foreach (var item in response.Items)
                {
                    yield return item;
                }

                lastEvaluatedKey = response.LastEvaluatedKey;
            } while (lastEvaluatedKey is not null && lastEvaluatedKey.Count != 0);
        }

        public static async Task PutAll(this IAmazonDynamoDB client, string tableName, IEnumerable<Dictionary<string, AttributeValue>> items)
        {
            var dynamoBatchSizeLimit = 25;
            var batches = items.Chunk(dynamoBatchSizeLimit);
            await Task.WhenAll(batches.Select(async batch =>
            {
                var writeRequests = batch.Select(_ =>
                {
                    return new WriteRequest
                    {
                        PutRequest = new PutRequest
                        {
                            Item = _
                        }
                    };
                }).ToList();

                await client.BatchWriteItemAsync(new BatchWriteItemRequest
                {
                    RequestItems = new Dictionary<string, List<WriteRequest>>
                    {
                        [tableName] = writeRequests
                    }
                });
            }));
        }
    }
}
