using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Hypercube.Host.DependencyGroups;
using Hypercube.Host.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Avvo.API.DependencyGroups;

/// <summary>
///     SwaggerDependencies Class
/// </summary>
public class SwaggerDependencies : IDependencyGroup
{
    /// <summary>
    ///     Register Method
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="serviceCollection">ServiceCollection</param>
    public void Register(ILogger logger, IServiceCollection serviceCollection)
    {
        serviceCollection.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressInferBindingSourcesForParameters = false;
        });

        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen(c =>
        {
            c.DescribeAllParametersInCamelCase();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            c.SchemaFilter<EnumSchemaFilter>();

        });

        serviceCollection.AddSwaggerGenNewtonsoftSupport();
    }
}
