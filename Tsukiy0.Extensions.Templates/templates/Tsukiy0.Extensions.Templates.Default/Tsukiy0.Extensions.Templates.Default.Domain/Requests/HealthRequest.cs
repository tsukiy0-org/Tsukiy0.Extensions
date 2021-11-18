using MediatR;

namespace Tsukiy0.Extensions.Templates.Default.Domain.Requests;

public record HealthRequest : IRequest<Unit> { }