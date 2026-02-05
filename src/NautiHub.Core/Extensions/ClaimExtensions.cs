using Microsoft.Extensions.Logging;
using NautiHub.Core.DomainObjects;
using System.Security.Claims;

namespace NautiHub.Core.Extensions;

public static class ClaimExtensions
{
    public static T? GetIdentity<T>(this ClaimsPrincipal user, ILogger logger)
        where T : INautiHubIdentity, new()
    {
        return user.Claims.GetIdentity<T>(logger);
    }

    public static T? GetIdentity<T>(this ClaimsIdentity identity, ILogger logger)
        where T : INautiHubIdentity, new()
    {
        return identity.Claims.GetIdentity<T>(logger);
    }

    public static T? GetIdentity<T>(this IEnumerable<Claim> claims, ILogger logger)
        where T : INautiHubIdentity, new()
    {
        if (claims == null || !claims.Any())
        {
            return default;
        }

        T result = new();
        try
        {
            Claim? userIdClaim = claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim?.Value != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                result.SetUserId(userId);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(
                ex,
                "ClaimsIdentityExtensions_GetIdentity : Could not load claim UserId"
            );
        }

        try
        {
            string? name = (
                from q in claims
                where q.Type.Contains("UserName")
                select q.Value
            ).LastOrDefault();
            result.SetName(name ?? string.Empty);
        }
        catch (Exception ex)
        {
            logger.LogWarning(
                ex,
                "ClaimsIdentityExtensions_GetIdentity : Could not load claim UserName"
            );
        }

        try
        {
            string? email = (
                from q in claims
                where q.Type.Contains("UserEmail")
                select q.Value
            ).FirstOrDefault();
            result.SetEmail(email ?? string.Empty);
        }
        catch (Exception ex)
        {
            logger.LogWarning(
                ex,
                "ClaimsIdentityExtensions_GetIdentity : Could not load claim UserEmail"
            );
        }

        return result;
    }
}
