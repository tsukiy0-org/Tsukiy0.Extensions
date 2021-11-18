using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Tsukiy0.Extensions.AspNetCore.Configs;
using Tsukiy0.Extensions.AspNetCore.Extensions;
using Tsukiy0.Extensions.Configuration.Extensions;

namespace Tsukiy0.Extensions.Example.AspNetCore
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
            services.AddDefaults();
            services.AddApiKeyAuth(Configuration);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tsukiy0.Extensions.Example.AspNetCore", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaults(Configuration);
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Tsukiy0.Extensions.Example.AspNetCore v1"));
        }
    }
}