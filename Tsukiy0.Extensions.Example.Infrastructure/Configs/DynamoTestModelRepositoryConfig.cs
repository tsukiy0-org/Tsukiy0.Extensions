namespace Tsukiy0.Extensions.Example.Infrastructure.Configs
{
    public record DynamoTestModelRepositoryConfig
    {
        public string TableName { get; init; }
    }
}