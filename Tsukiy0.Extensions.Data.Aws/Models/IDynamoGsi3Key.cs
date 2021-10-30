namespace Tsukiy0.Extensions.Data.Aws.Models
{
    public interface IDynamoGsi3Key
    {
        string __GSI3_PK { get; }
        string __GSI3_SK { get; }
    }
}
