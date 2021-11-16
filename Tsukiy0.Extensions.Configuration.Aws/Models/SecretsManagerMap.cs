namespace Tsukiy0.Extensions.Configuration.Aws.Models
{
    public record SecretsManagerMap
    {
        public string SecretName { get; init; }
        public string SecretKey { get; init; }
        public string ConfigurationKey { get; init; }
    }
}