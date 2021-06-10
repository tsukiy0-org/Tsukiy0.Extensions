using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Tsukiy0.Extensions.AspNetCore.Configs;

namespace Tsukiy0.Extensions.AspNetCore.Filters
{
    public class ApiKeyAuthFilter : IAuthorizationFilter
    {
        public const string Header = "x-api-key";
        private readonly ApiKeyAuthConfig config;

        public ApiKeyAuthFilter(ApiKeyAuthConfig config)
        {
            this.config = config;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var headers = context.HttpContext.Request.Headers[Header].ToArray();
            var keys = config.ApiKeys.ToList().Select(_ => _.Value);

            var ok = headers.FirstOrDefault(header => keys.Any(key => string.Equals(header, key))) != null;

            if (!ok)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
