using System.Collections.Generic;

namespace Tsukiy0.Extensions.AspNetCore.Configs
{
    public class ApiKeyAuthConfig
    {
        public IDictionary<string, string> ApiKeys { get; init; }
    }
}