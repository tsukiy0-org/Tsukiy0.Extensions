using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace Tsukiy0.Extensions.MediatR.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatR(this IServiceCollection _)
        {
            _.AddScoped<ServiceFactory>(_ => type => _.GetService(type));
            _.AddScoped<IMediator, Mediator>();
            return _;
        }
    }
}