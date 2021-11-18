using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Amazon.SQS;
using Amazon.SQS.Model;

using Tsukiy0.Extensions.Core.Extensions;
using Tsukiy0.Extensions.Json.Extensions;
using Tsukiy0.Extensions.Messaging.Models;
using Tsukiy0.Extensions.Messaging.Services;

namespace Tsukiy0.Extensions.Messaging.Aws.Services
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

        public async Task Send(IEnumerable<Message<T>> messages)
        {
            var requests = messages.Chunk(10).Select(chunk => new SendMessageBatchRequest
            {
                QueueUrl = queueUrl,
                Entries = chunk.Select(_ => new SendMessageBatchRequestEntry
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageBody = JsonSerializer.Serialize(_, JsonSerializerExtensions.DefaultOptions)
                }).ToList()
            });

            await requests.Select(_ => client.SendMessageBatchAsync(_))
                .WhenAllBatched(5);

            // @TODO retry failed
        }
    }
}