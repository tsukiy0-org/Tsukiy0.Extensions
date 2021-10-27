using System;
using Tsukiy0.Extensions.Configuration.Aws.Models;

namespace Tsukiy0.Extensions.Configuration.Aws.Exceptions
{
    public class SecretNotFoundException : Exception
    {
        public SecretNotFoundException(SecretsManagerMap map) : base($"Missing parameter with key {map.SecretKey} and field {map.SecretField}")
        {
            Data.Add(nameof(map), map);
        }
    }
}
