using System;
using Amazon.Lambda.Core;
using Tsukiy0.Extensions.Processor.Aws.Runtimes;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Tsukiy0.Extensions.Example.Processor.Aws.Sqs
{
    public class Function : SqsLambdaProcessorRuntime<Guid>
    {
        public Function() : base(new Processor()) { }
    }
}
