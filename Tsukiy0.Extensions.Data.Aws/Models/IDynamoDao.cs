namespace Tsukiy0.Extensions.Data.Aws.Models
{
    public interface IDynamoDao
    {
        string __PK { get; }
        string __SK { get; }
        string __TYPE { get; }
        string __VERSION { get; }
    }
}
