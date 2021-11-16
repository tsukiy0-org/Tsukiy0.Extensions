using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Amazon.Lambda.SQSEvents;

using Tsukiy0.Extensions.Json.Extensions;
using Tsukiy0.Extensions.Messaging.Models;
using Tsukiy0.Extensions.Processor.Services;

namespace Tsukiy0.Extensions.Processor.Aws.Runtimes
{
    public class SqsLambdaProcessorRuntime<T> : LambdaProcessorRuntime<SQSEvent, T>
    {
        public SqsLambdaProcessorRuntime(IProcessor<T> processor) : base(processor) { }

        protected override Task<Message<T>> ToMessage(SQSEvent e)
        {
            return Task.FromResult(JsonSerializer.Deserialize<Message<T>>(e.Records.Single().Body, JsonSerializerExtensions.DefaultOptions));
        }
    }
}