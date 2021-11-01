namespace Tsukiy0.Extensions.Example.Infrastructure.Configs
{
    public record BatchSaveTestModelQueueConfig
    {
        public string JobDefinitionArn { get; set; }
        public string JobQueueArn { get; set; }
    }
}
