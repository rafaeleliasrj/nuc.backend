using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Avvo.Core.Host.Filters
{

    public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var atributePipeline = context.ApiDescription.ActionDescriptor.EndpointMetadata;

            var isAuthorized = context.ApiDescription.CustomAttributes().Any(c => c is AuthorizeAttribute);
            var allowAnonymous = context.ApiDescription.CustomAttributes().Any(c => c is AllowAnonymousAttribute);

            if (isAuthorized && !allowAnonymous)
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                //TODO Rever pois esta conflitando no swagger
                // operation.Parameters.Add(new OpenApiParameter
                // {
                //     Name = "Authorization",
                //     In = ParameterLocation.Header,
                //     Description = "Access Token",
                //     Required = true,
                //     Schema = new OpenApiSchema
                //     {
                //         Type = "String",
                //     }
                // });
            }
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-api-key",
                In = ParameterLocation.Header,
                Description = "Your Api Key",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String",
                }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-access-token",
                In = ParameterLocation.Header,
                Description = "The IDP Access Token",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String",
                }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-csrf-token",
                In = ParameterLocation.Header,
                Description = "CSRF Protection",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String",
                    Default = new OpenApiString(Guid.NewGuid().ToString())
                }
            });
        }
    }
}
