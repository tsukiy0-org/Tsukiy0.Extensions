using System;

using Microsoft.AspNetCore.Mvc;

using Tsukiy0.Extensions.AspNetCore.Filters;
using Tsukiy0.Extensions.Core.Exceptions;

namespace Tsukiy0.Extensions.Example.AspNetCore.Controllers
{
    [ApiController]
    [Route("v1/Exceptions")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiKeyAuth]
    public class ExceptionController : ControllerBase
    {
        [HttpGet("Validation")]
        public void Validation()
        {
            throw new ValidationException("");
        }

        [HttpGet("NotFound")]
        public void NotFound()
        {
            throw new NotFoundException("");
        }

        [HttpGet("Unknown")]
        public void Unknown()
        {
            throw new SystemException("");
        }
    }
}