using MediatR;

using Tsukiy0.Extensions.Templates.Default.Domain.Requests;

namespace Tsukiy0.Extensions.Templates.Default.Core.Handlers;

public class HealthHandler : IRequestHandler<HealthRequest, Unit>
{
    public Task<Unit> Handle(HealthRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Unit.Value);
    }
}