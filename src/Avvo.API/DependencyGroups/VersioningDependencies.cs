using Hypercube.Host.DependencyGroups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Avvo.API.DependencyGroups;

/// <summary>
///     VersioningDependencies Class
/// </summary>
public class VersioningDependencies : IDependencyGroup
{
    /// <summary>
    ///     Register Method
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="serviceCollection">ServiceCollection</param>
    public void Register(ILogger logger, IServiceCollection serviceCollection)
    {
        serviceCollection.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        });

        serviceCollection.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        serviceCollection.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    }
}
