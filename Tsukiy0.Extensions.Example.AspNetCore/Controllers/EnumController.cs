using Microsoft.AspNetCore.Mvc;
using Tsukiy0.Extensions.AspNetCore.Filters;
using Tsukiy0.Extensions.Example.AspNetCore.Models;

namespace Tsukiy0.Extensions.Example.AspNetCore.Controllers
{
    [ApiController]
    [Route("v1/Enums")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiKeyAuth]
    public class EnumsController : ControllerBase
    {
        [HttpGet]
        public Fruit Get(Fruit fruit)
        {
            return fruit;
        }
    }
}

