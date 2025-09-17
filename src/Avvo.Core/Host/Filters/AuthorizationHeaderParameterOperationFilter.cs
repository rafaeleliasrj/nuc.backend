using Microsoft.AspNetCore.Authorization;
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
            }
        }
    }
}
