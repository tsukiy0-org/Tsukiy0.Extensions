using Microsoft.AspNetCore.Mvc;
using Tsukiy0.Extensions.AspNetCore.Configs;
using Tsukiy0.Extensions.AspNetCore.Filters;

namespace Tsukiy0.Extensions.TestBed.AspNetCore.Controllers
{
    [ApiController]
    [Route("Config")]
    [TypeFilter(typeof(ApiKeyAuthFilter))]
    public class ConfigController : ControllerBase
    {
        private readonly ApiKeyAuthConfig config;

        public ConfigController(ApiKeyAuthConfig config)
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

