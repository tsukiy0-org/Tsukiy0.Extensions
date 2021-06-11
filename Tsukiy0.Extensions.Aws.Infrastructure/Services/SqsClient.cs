using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Tsukiy0.Extensions.Aws.Core.Models;
using Tsukiy0.Extensions.Aws.Core.Services;
using Tsukiy0.Extensions.Core.Extensions;

namespace Tsukiy0.Extensions.Aws.Infrastructure.Services
{
    public class SqsClient<T> : ISqsClient<T>
    {
        private readonly IAmazonSQS client;
        private readonly Uri queueUrl;

        public SqsClient(IAmazonSQS client, Uri queueUrl)
        {
            this.client = client;
            this.queueUrl = queueUrl;
        }

        public async Task Send(IEnumerable<SendMessageEnvelope<T>> messages)
        {
            var tasks = messages
                .Select(_ =>
                {
                    return new SendMessageBatchRequestEntry
                    {
                        Id = Guid.NewGuid().ToString(),
                        MessageBody = JsonSerializer.Serialize(_)
                    };
                })
                .Chunk(10)
                .Select(async _ =>
                {
                    var res = await client.SendMessageBatchAsync(new SendMessageBatchRequest
                    {
                        QueueUrl = queueUrl.ToString(),
                        Entries = _.ToList()
                    });

                    if (res.Failed.Any())
                    {
                        throw new Exception();
                    }
                });

            await Task.WhenAll(tasks);
        }

        public async Task<ReceiveMessageEnvelope<T>?> Receive()
        {
            var res = await client.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = queueUrl.ToString(),
                MaxNumberOfMessages = 1
            });

            return res.Messages.Select(_ =>
            {
                var env = JsonSerializer.Deserialize<SendMessageEnvelope<T>>(_.Body);
                return new ReceiveMessageEnvelope<T>(_.ReceiptHandle, env.TraceId, env.Message);
            }).FirstOrDefault();
        }

        public async Task Delete(string receiptHandle)
        {
            await client.DeleteMessageAsync(new DeleteMessageRequest
            {
                QueueUrl = queueUrl.ToString(),
                ReceiptHandle = receiptHandle
            });
        }
    }
}
