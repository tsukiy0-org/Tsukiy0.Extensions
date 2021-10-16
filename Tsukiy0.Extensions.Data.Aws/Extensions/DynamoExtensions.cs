using System.Collections.Generic;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
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
            return Document.FromJson(JsonSerializer.Serialize(input, JsonSerializerExtensions.DefaultJsonSerializerOptions)).ToAttributeMap();
        }

        public static T FromAttributeMap<T>(this Dictionary<string, AttributeValue> input)
        {
            return JsonSerializer.Deserialize<T>(Document.FromAttributeMap(input).ToJson(), JsonSerializerExtensions.DefaultJsonSerializerOptions);
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
    }
}
