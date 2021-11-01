using Amazon.Lambda.Core;
using Tsukiy0.Extensions.Example.Core.Handlers;
using Tsukiy0.Extensions.Processor.Aws.Runtimes;
using Tsukiy0.Extensions.Example.Infrastructure.Services;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Tsukiy0.Extensions.Example.Processor.Aws.Sqs
{
    public class Function : SqsLambdaProcessorRuntime<SaveTestModelRequest>
    {
        public Function() : base(new SaveTestModelProcessor()) { }
    }
}
