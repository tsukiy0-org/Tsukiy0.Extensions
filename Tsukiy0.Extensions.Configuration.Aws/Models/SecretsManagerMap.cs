namespace Tsukiy0.Extensions.Configuration.Aws.Models
{
    public record SecretsManagerMap(string SecretName, string SecretKey, string ConfigurationKey);
}
