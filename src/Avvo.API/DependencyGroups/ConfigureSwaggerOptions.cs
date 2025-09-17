using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Avvo.API.DependencyGroups;

/// <summary>
///     Configures the Swagger generation options.
/// </summary>
/// <remarks>
///     This allows API versioning to define a Swagger document per API version after the
///     <see cref="IApiVersionDescriptionProvider" /> service has been resolved from the service container.
/// </remarks>
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private const string URI = "https://github.com/rafaeleliasrj/nuc.backend";

    /// <summary>
    ///     Provider
    /// </summary>
    public readonly IApiVersionDescriptionProvider _provider;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConfigureSwaggerOptions" /> class.
    /// </summary>
    /// <param name="provider">
    ///     The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger
    ///     documents.
    /// </param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        // add a swagger document for each discovered API version
        // note: you might choose to skip or document deprecated API versions differently
        foreach (var description in _provider.ApiVersionDescriptions)
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
    }

    /// <summary>
    ///     Creates the <see cref="OpenApiInfo" /> for the given <paramref name="description" />.
    /// </summary>
    /// <param name="description">Description</param>
    /// <returns></returns>
    public static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Version = description.GroupName,
            Title = "Avvo Sales API",
            Description = "Avvo Sales Service API",
            TermsOfService = new Uri($"{URI}/TermsOfService"),
            Contact = new OpenApiContact
            {
                Name = "Rafael Meireles Elias",
                Email = "rafael.elias@outlook.com",
                Url = new Uri($"{URI}/Contact")
            },
            License = new OpenApiLicense
            {
                Name = "Use under MIT License",
                Url = new Uri($"{URI}/License")
            }
        };

        if (description.IsDeprecated) info.Description += " This API version has been deprecated.";

        return info;
    }
}
