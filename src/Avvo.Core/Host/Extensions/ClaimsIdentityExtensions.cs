using System.Security.Claims;
using Avvo.Core.Commons.Interfaces;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Host.Extensions
{
    public static class ClaimsIdentityExtensions
    {
        public static T GetIdentity<T>(this ClaimsPrincipal user, ILogger logger) where T : IUserIdentity, new() => user.Claims.GetIdentity<T>(logger);
        public static T GetIdentity<T>(this ClaimsIdentity identity, ILogger logger) where T : IUserIdentity, new() => identity.Claims.GetIdentity<T>(logger);
        public static T GetIdentity<T>(this IEnumerable<Claim> claims, ILogger logger) where T : IUserIdentity, new()
        {
            if (claims == null || claims.Count() == 0)
                return default(T);

            var identity = new T();

            try
            {
                var userId = Guid.Parse(claims.FirstOrDefault(c => c.Type == "UserId").Value.ToString());
                identity.UserId = userId;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, $"ClaimsIdentityExtensions_GetIdentity : Could not load claim UserId");
            }

            try
            {
                identity.Name = claims.Where(c => c.Type.Contains("UserName")).Select(q => q.Value).LastOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, $"ClaimsIdentityExtensions_GetIdentity : Could not load claim UserName");
            }

            try
            {
                identity.Email = claims.Where(c => c.Type.Contains("UserEmail")).Select(q => q.Value).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, $"ClaimsIdentityExtensions_GetIdentity : Could not load claim UserEmail");
            }

            try
            {
                identity.SubscriptionId = Guid.Parse(claims.Where(c => c.Type.Contains("SubscriptionId")).Select(q => q.Value).FirstOrDefault());
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, $"ClaimsIdentityExtensions_GetIdentity : Could not load claim SubscriptionId");
            }


            try
            {
                var scopes = claims.Where(c => c.Type.Contains("Scopes")).Select(q => q.Value).FirstOrDefault();
                identity.Scopes = scopes?.Split(',')?.ToList();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, $"ClaimsIdentityExtensions_GetIdentity : Could not load claim Scopes");
            }

            try
            {
                var accessToken = claims.FirstOrDefault(c => c.Type == "access_token")?.Value;
                identity.AccessToken = accessToken;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, $"ClaimsIdentityExtensions_GetIdentity : Could not load claim AccessToken");
            }

            return identity;
        }
    }
}
