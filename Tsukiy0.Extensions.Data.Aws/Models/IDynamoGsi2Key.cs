namespace Tsukiy0.Extensions.Data.Aws.Models
{
    public interface IDynamoGsi2Key
    {
        string __GSI2_PK { get; }
        string __GSI2_SK { get; }
    }
}
