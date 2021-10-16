using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Tsukiy0.Extensions.Core.Exceptions;
using Tsukiy0.Extensions.Json.Extensions;

namespace Tsukiy0.Extensions.AspNetCore.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception e)
        {
            logger.LogError(e, "Request exception");

            var statusCode = e switch
            {
                ValidationException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError,
            };

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";
            var body = JsonSerializer.Serialize(new
            {
                Message = e.Message
            }, JsonSerializerExtensions.DefaultJsonSerializerOptions);
            await context.Response.WriteAsync(body);
        }
    }
}
