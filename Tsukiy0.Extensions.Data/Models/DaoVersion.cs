namespace Tsukiy0.Extensions.Data.Models
{
    public record DaoVersion
    {
        public string Type { get; init; }
        public int Version { get; init; }
    }
}
