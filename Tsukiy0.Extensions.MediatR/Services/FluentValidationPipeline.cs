using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

namespace Tsukiy0.Extensions.MediatR.Services
{
    public class FluentValidationPipeline<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator> _validators;

        public FluentValidationPipeline(IEnumerable<IValidator> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationFailures = _validators
                    .Select(validator => validator.Validate(context))
                    .SelectMany(validationResult => validationResult.Errors)
                    .Where(validationFailure => validationFailure != null)
                    .ToList();

                if (validationFailures.Any())
                {
                    throw new ValidationException(validationFailures.First().ErrorMessage);
                }
            }

            return next();
        }
    }
}