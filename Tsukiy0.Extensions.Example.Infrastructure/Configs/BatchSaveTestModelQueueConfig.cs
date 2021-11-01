using System;

namespace Tsukiy0.Extensions.Example.Infrastructure.Configs
{
    public record BatchSaveTestModelQueueConfig
    {
        public string JobDefinitionArn { get; set; }
        public string JobQueueArn { get; set; }

        public static object JobQueueUrl()
        {
            throw new NotImplementedException();
        }
    }
}
