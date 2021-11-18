using Microsoft.AspNetCore.Mvc;

using Tsukiy0.Extensions.AspNetCore.Configs;
using Tsukiy0.Extensions.AspNetCore.Filters;

namespace Tsukiy0.Extensions.AspNetCore.Tests.TestApi.Controllers
{
    [ApiController]
    [Route("v1/Auth")]
    [ApiKeyAuth]
    public class AuthController : ControllerBase
    {
        private readonly ApiKeyAuthConfig config;

        public AuthController(ApiKeyAuthConfig config)
        {
            this.config = config;
        }

        [HttpGet]
        public ApiKeyAuthConfig GetConfig()
        {
            return config;
        }
    }
}