using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.SQSEvents;
using Tsukiy0.Extensions.Json.Extensions;
using Tsukiy0.Extensions.Processor.Models;
using Tsukiy0.Extensions.Processor.Services;

namespace Tsukiy0.Extensions.Processor.Aws.Runtimes
{
    public class SqsLambdaProcessorRuntime<T> : LambdaProcessorRuntime<SQSEvent, T>
    {
        public SqsLambdaProcessorRuntime(IProcessor<T> processor) : base(processor) { }

        protected override async Task<Message<T>> ToMessage(SQSEvent e)
        {
            return JsonSerializer.Deserialize<Message<T>>(e.Records.Single().Body, JsonSerializerExtensions.DefaultJsonSerializerOptions);
        }
    }
}
