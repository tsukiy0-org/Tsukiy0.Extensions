namespace Tsukiy0.Extensions.Example.Infrastructure.Configs
{
    public record BatchSaveTestModelQueueConfig
    {
        public string JobDefinitionArn { get; init; }
        public string JobQueueArn { get; init; }
    }
}
