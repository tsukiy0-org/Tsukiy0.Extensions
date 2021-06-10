using Microsoft.AspNetCore.Mvc;
using Tsukiy0.Extensions.AspNetCore.Configs;

namespace Tsukiy0.Extensions.TestBed.AspNetCore.Controllers
{
    [ApiController]
    [Route("Config")]
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

