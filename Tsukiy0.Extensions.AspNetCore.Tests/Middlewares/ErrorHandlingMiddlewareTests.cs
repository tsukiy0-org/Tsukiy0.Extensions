using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.Json;
using Tsukiy0.Extensions.AspNetCore.Middlewares;
using Tsukiy0.Extensions.Logging.NLog.Tests.Helpers;
using Tsukiy0.Extensions.Core.Exceptions;
using Moq;

namespace Tsukiy0.Extensions.AspNetCore.Tests.Middlewares
{
    public class ErrorHandlingMiddlewareTests
    {
        private readonly ErrorHandlingMiddleware sut;

        public ErrorHandlingMiddlewareTests()
        {
            var logHelper = new MemoryLoggerHelper<ErrorHandlingMiddleware>();
            sut = new ErrorHandlingMiddleware(logHelper.Logger);
        }

        [Theory]
        [MemberData(nameof(Exceptions))]
        public async void HandleKnownExceptions(Exception e, HttpStatusCode statusCode)
        {
            var mockRequestDelegate = new Mock<RequestDelegate>();
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            mockRequestDelegate.Setup(_ => _.Invoke(context)).ThrowsAsync(e);

            await sut.InvokeAsync(context, mockRequestDelegate.Object);

            context.Response.StatusCode.Should().Be((int)statusCode);
            context.Response.ContentType.Should().Be("application/json");
        }

        public static IEnumerable<object[]> Exceptions =>
            new List<object[]>
            {
                new object[] { new ValidationException("validation"), HttpStatusCode.BadRequest },
                new object[] { new NotFoundException("not found"), HttpStatusCode.NotFound },
                new object[] { new Exception("generic"), HttpStatusCode.InternalServerError },
            };
    }
}
