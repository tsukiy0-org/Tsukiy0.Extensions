
using Amazon.Batch;
using Tsukiy0.Extensions.Correlation.Services;
using Tsukiy0.Extensions.Example.Core.Handlers;
using Tsukiy0.Extensions.Example.Infrastructure.Configs;
using Tsukiy0.Extensions.Messaging.Aws.Services;
using Tsukiy0.Extensions.Messaging.Services;

namespace Tsukiy0.Extensions.Example.Infrastructure.Services
{
    public class BatchSaveTestModelQueue : DefaultQueue<SaveTestModelRequest>
    {
        public BatchSaveTestModelQueue(IAmazonBatch batch, BatchSaveTestModelQueueConfig config, ICorrelationService correlationService)
        : base(new BatchMessageQueue<SaveTestModelRequest>(batch, config.JobQueueArn, config.JobDefinitionArn), correlationService)
        {
        }
    }

}
