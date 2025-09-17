using Avvo.Core.Commons.Jwt;
using Avvo.Core.Host.Extensions;
using Avvo.Core.Host.DependencyGroups;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Avvo.API.DependencyGroups;

/// <summary>
///     Authentication Dependencies Class
/// </summary>
public class AuthenticationDependencies : IDependencyGroup
{
    /// <summary>
    ///     Register Dependencies
    /// </summary>
    /// <param name="logger">Logger Object</param>
    /// <param name="serviceCollection">Service Collection Object</param>
    public void Register(ILogger logger, IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddAvvoJwtBearer(JwtSettingsFactory.CreateFromEnvironmentVariables());
    }
}
