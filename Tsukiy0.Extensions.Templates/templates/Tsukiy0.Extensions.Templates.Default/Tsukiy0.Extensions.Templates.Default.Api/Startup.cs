using MediatR;

using Tsukiy0.Extensions.AspNetCore.Extensions;
using Tsukiy0.Extensions.MediatR.Extensions;
using Tsukiy0.Extensions.Templates.Default.Core.Handlers;
using Tsukiy0.Extensions.Templates.Default.Domain.Requests;

namespace Tsukiy0.Extensions.Templates.Default.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDefaults();
        services.AddMediatR();
        services.AddSwaggerGen();

        services.AddScoped<IRequestHandler<HealthRequest, Unit>, HealthHandler>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseDefaults(Configuration);
        app.UseSwaggerUI();
    }
}