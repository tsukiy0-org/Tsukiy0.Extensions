namespace Tsukiy0.Extensions.Data.Aws.Models
{
    public interface IDynamoGsi1Key
    {
        string __GSI1_PK { get; }
        string __GSI1_SK { get; }
    }

    public record DynamoGsi1Key(string __GSI1_PK, string __GSI1_SK) : IDynamoGsi1Key { }
}
