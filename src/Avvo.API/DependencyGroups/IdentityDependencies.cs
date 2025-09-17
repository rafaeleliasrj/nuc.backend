using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Interfaces;
using Avvo.Core.Host.DependencyGroups;
using Avvo.Core.Host.Extensions;

namespace Avvo.API.DependencyGroups;

/// <summary>
///     IdentityDependencies Class
/// </summary>
public class IdentityDependencies : IDependencyGroup
{
    /// <summary>
    ///     Register Method
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="serviceCollection">ServiceCollection</param>
    public void Register(ILogger logger, IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpContextAccessor();

        serviceCollection.AddScoped<IUserIdentity>(provider =>
        {
            var context = provider.GetService<IHttpContextAccessor>()?.HttpContext;
            var req = context?.Request;
            var claims = context?.User?.Claims;

            if (claims != null && claims.Any())
            {
                var identity = claims.GetIdentity<UserIdentity>(logger);

                return identity;
            }

            return null;
        });
    }
}
