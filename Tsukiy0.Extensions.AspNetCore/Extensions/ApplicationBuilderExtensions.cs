using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Tsukiy0.Extensions.AspNetCore.Middlewares;
using Tsukiy0.Extensions.Configuration.Extensions;
using Tsukiy0.Extensions.Logging.AspNetCore.Middlewares;

namespace Tsukiy0.Extensions.AspNetCore.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDefaultLogging(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<LogCorrelationMiddleware>()
                .UseMiddleware<LogRequestMiddleware>();
        }

        public static IApplicationBuilder UseCorsWhitelist(this IApplicationBuilder app, IConfiguration configuration)
        {
            var origins = configuration.GetSection<string[]>("CorsWhitelist");

            if (origins is null)
            {
                return app;
            }

            return app.UseCors(_ =>
            {
                _.SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        }

        public static void UseErrorHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }

        public static void UseDefaults(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseDefaultLogging();
            app.UseErrorHandling();

            app.UseSwagger();
            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseRouting();
            app.UseCorsWhitelist(configuration);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
