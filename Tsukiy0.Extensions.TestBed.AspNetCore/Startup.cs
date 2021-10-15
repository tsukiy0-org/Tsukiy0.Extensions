using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Tsukiy0.Extensions.AspNetCore.Extensions;
using Tsukiy0.Extensions.AspNetCore.Filters;

namespace Tsukiy0.Extensions.TestBed.AspNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiKeyAuth(Configuration);
            services.AddDefaults();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tsukiy0.Extensions.TestBed.AspNetCore", Version = "v1" });
                c.AddAuthHeader(ApiKeyAuthFilter.Header);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaults(Configuration);
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tsukiy0.Extensions.TestBed.AspNetCore v1"));
        }
    }
}
