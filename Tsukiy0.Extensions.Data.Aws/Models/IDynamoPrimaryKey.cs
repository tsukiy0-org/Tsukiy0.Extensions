namespace Tsukiy0.Extensions.Data.Aws.Models
{
    public interface IDynamoPrimaryKey
    {
        string __PK { get; }
        string __SK { get; }
    }

    public record DynamoPrimaryKey(string __PK, string __SK) : IDynamoPrimaryKey { }
}
