using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Batch;
using Amazon.Batch.Model;
using Tsukiy0.Extensions.Json.Extensions;
using Tsukiy0.Extensions.Processor.Models;
using Tsukiy0.Extensions.Processor.Services;

namespace Tsukiy0.Extensions.Processor.Aws.Services
{
    public class BatchMessageQueue<T> : IMessageQueue<T>
    {
        public const string MESSAGE_KEY = "MESSAGE_25a9ae4a5d754eb8a0526e7ad2c05801";
        private readonly IAmazonBatch _client;
        private readonly string _jobQueueArn;
        private readonly string _jobDefinitionArn;

        public BatchMessageQueue(IAmazonBatch client, string jobQueueArn, string jobDefinitionArn)
        {
            _client = client;
            _jobQueueArn = jobQueueArn;
            _jobDefinitionArn = jobDefinitionArn;
        }

        public async Task Send(Message<T> message)
        {
            await _client.SubmitJobAsync(new SubmitJobRequest
            {
                JobName = message.Header.TraceId.ToString(),
                JobQueue = _jobQueueArn,
                JobDefinition = _jobDefinitionArn,
                ContainerOverrides = new ContainerOverrides
                {
                    Environment = new List<Amazon.Batch.Model.KeyValuePair>
                    {
                        new Amazon.Batch.Model.KeyValuePair {
                            Name = MESSAGE_KEY,
                            Value = JsonSerializer.Serialize(message, JsonSerializerExtensions.DefaultJsonSerializerOptions)
                        }
                    }
                }
            });
        }

        public async Task Send(IEnumerable<Message<T>> messages)
        {
            await Task.WhenAll(messages.Select(async _ =>
            {
                await Send(_);
            }));
        }
    }
}
