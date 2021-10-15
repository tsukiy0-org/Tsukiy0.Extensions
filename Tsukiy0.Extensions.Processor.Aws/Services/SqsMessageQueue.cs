using System.Text.Json;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
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

        public async Task Send(Message<T> message)
        {
            await client.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = JsonSerializer.Serialize(message)
            });
        }
    }
}
