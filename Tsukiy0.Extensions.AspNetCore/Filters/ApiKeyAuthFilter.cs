using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using Tsukiy0.Extensions.AspNetCore.Configs;
using Tsukiy0.Extensions.Http.Constants;

namespace Tsukiy0.Extensions.AspNetCore.Filters
{
    public class ApiKeyAuthFilter : IAuthorizationFilter
    {
        private readonly ApiKeyAuthConfig _config;
        private readonly ILogger<ApiKeyAuthFilter> _logger;

        public ApiKeyAuthFilter(ApiKeyAuthConfig config, ILogger<ApiKeyAuthFilter> logger)
        {
            _config = config;
            _logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var headers = context.HttpContext.Request.Headers[HttpHeaders.ApiKey].ToArray();
            var keys = _config.ApiKeys.ToList();

            var key = keys.FirstOrDefault(key => headers.Any(header => string.Equals(header, key.Value)));

            _logger.LogInformation("Authenticated with api key {key}", key.Key);

            if (key.Key is null)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }

    public class ApiKeyAuthAttribute : TypeFilterAttribute
    {
        public ApiKeyAuthAttribute() : base(typeof(ApiKeyAuthFilter)) { }
    }
}