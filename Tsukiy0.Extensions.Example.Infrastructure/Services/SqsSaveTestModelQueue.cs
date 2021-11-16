
using Amazon.SQS;

using Tsukiy0.Extensions.Correlation.Services;
using Tsukiy0.Extensions.Example.Core.Handlers;
using Tsukiy0.Extensions.Example.Infrastructure.Configs;
using Tsukiy0.Extensions.Messaging.Aws.Services;
using Tsukiy0.Extensions.Messaging.Services;

namespace Tsukiy0.Extensions.Example.Infrastructure.Services
{
    public class SqsSaveTestModelQueue : DefaultQueue<SaveTestModelRequest>
    {
        public SqsSaveTestModelQueue(IAmazonSQS sqs, SqsSaveTestModelQueueConfig config, ICorrelationService correlationService)
        : base(new SqsMessageQueue<SaveTestModelRequest>(sqs, config.QueueUrl), correlationService)
        {
        }
    }

}