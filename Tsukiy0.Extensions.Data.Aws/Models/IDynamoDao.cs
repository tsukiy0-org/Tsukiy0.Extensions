namespace Tsukiy0.Extensions.Data.Aws.Models
{
    public interface IDynamoDao : IDynamoPrimaryKey
    {
        string __PK { get; }
        string __SK { get; }
        string __TYPE { get; }
        int __VERSION { get; }
    }
}
