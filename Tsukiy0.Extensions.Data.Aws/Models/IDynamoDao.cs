namespace Tsukiy0.Extensions.Data.Aws.Models
{
    public interface IDynamoDao : IDynamoPrimaryKey
    {
        new string __PK { get; }
        new string __SK { get; }
        string __TYPE { get; }
        int __VERSION { get; }
    }
}