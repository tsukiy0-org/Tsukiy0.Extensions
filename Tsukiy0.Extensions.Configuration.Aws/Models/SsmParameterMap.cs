namespace Tsukiy0.Extensions.Configuration.Aws.Models
{
    public record SsmParameterMap
    {
        public string ParameterKey { get; init; }
        public string ConfigurationKey { get; init; }
    };
}
