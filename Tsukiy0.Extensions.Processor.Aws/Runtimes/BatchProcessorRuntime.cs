using System;
using System.Text.Json;
using System.Threading.Tasks;

using Tsukiy0.Extensions.Json.Extensions;
using Tsukiy0.Extensions.Messaging.Aws.Services;
using Tsukiy0.Extensions.Messaging.Models;
using Tsukiy0.Extensions.Processor.Services;

namespace Tsukiy0.Extensions.Processor.Aws.Runtimes
{
    public class BatchProcessorRuntime<T>
    {
        private readonly IProcessor<T> _processor;

        public BatchProcessorRuntime(IProcessor<T> processor)
        {
            _processor = processor;
        }

        public async Task Run()
        {
            var messageRaw = Environment.GetEnvironmentVariable(BatchMessageQueue<T>.MESSAGE_KEY);
            var message = JsonSerializer.Deserialize<Message<T>>(messageRaw, JsonSerializerExtensions.DefaultOptions);
            await _processor.Run(message);
        }
    }
}