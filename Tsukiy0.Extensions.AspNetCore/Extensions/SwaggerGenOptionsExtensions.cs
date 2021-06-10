using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tsukiy0.Extensions.AspNetCore
{
    public static class SwaggerGenOptionsExtensions
    {
        public static void AddAuthHeader(this SwaggerGenOptions options, string keyName)
        {
            options.AddSecurityDefinition(keyName, new OpenApiSecurityScheme()
            {
                Name = keyName,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            });

            var key = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = keyName
                },
                In = ParameterLocation.Header
            };

            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { key, new List<string>() }
            });
        }
    }
}
