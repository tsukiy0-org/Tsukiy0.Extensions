using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using FluentValidation;

using MediatR;

using Tsukiy0.Extensions.MediatR.Services;

using Xunit;

namespace Tsukiy0.Extensions.MediatR.Tests
{
    public class FluentValidationPipelineTests
    {
        private readonly FluentValidationPipeline<TestRequest, TestResponse> _sut;

        public FluentValidationPipelineTests()
        {
            var validators = new List<IValidator<TestRequest>> {
                new TestRequestValidator()
            };

            _sut = new FluentValidationPipeline<TestRequest, TestResponse>(validators);
        }

        [Fact]
        public async Task WhenValidThenDoNotThrow()
        {
            // Arrange
            var request = new TestRequest { Code = 1 };

            // Act
            Func<Task> action = async () => await _sut.Handle(request, CancellationToken.None, () => Task.FromResult(new TestResponse { Code = 1 }));

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WhenNotValidThenThrowAsync()
        {
            // Arrange
            var request = new TestRequest { Code = 0 };

            // Act
            Func<Task> action = async () => await _sut.Handle(request, CancellationToken.None, () => Task.FromResult(new TestResponse { Code = 1 }));

            // Assert
            await action.Should().ThrowAsync<ValidationException>();
        }

        private record TestResponse
        {
            public int Code { get; init; }
        }
        private record TestRequest : IRequest<TestResponse>
        {
            public int Code { get; init; }
        }
        private class TestRequestValidator : AbstractValidator<TestRequest>
        {
            public TestRequestValidator()
            {
                RuleFor(_ => _.Code).NotEmpty();
            }
        }
    }
}