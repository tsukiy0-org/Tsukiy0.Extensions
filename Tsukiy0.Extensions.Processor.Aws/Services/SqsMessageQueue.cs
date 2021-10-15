using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Tsukiy0.Extensions.Json.Extensions;
using Tsukiy0.Extensions.Processor.Models;
using Tsukiy0.Extensions.Processor.Services;

namespace Tsukiy0.Extensions.Processor.Aws.Services
{
    public class SqsMessageQueue<T> : IMessageQueue<T>
    {
        private readonly IAmazonSQS client;
        private readonly string queueUrl;

        public SqsMessageQueue(IAmazonSQS client, string queueUrl)
        {
            this.client = client;
            this.queueUrl = queueUrl;
        }

        public async Task Send(params Message<T>[] messages)
        {
            await client.SendMessageBatchAsync(new SendMessageBatchRequest
            {
                QueueUrl = queueUrl,
                Entries = messages.Select(_ => new SendMessageBatchRequestEntry
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageBody = JsonSerializer.Serialize(_, JsonSerializerExtensions.DefaultJsonSerializerOptions)
                }).ToList()
            });

            // @TODO retry failed
        }
    }
}
