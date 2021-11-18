namespace Tsukiy0.Extensions.Data.Aws.Models
{
    public record DynamoKey
    {
        public string PK { get; init; }
        public string SK { get; init; }
    }
}