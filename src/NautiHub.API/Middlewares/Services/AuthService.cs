using NautiHub.Core.Mediator;
using NautiHub.Domain.Services.DomainService;
using System.IdentityModel.Tokens.Jwt;

namespace NautiHub.Common.Middlewares.Services;

public class AuthService(
    IMediatorHandler mediator,
    IHttpContextAccessor accessor
) : IAuthService
{
    private readonly IMediatorHandler _mediator = mediator;
    private readonly IHttpContextAccessor _accessor = accessor;

    public Guid GetUserId()
    {
        var authorizationHeader = _accessor.HttpContext!.Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            return Guid.Empty;

        var accessToken = authorizationHeader["Bearer ".Length..].Trim();

        if (string.IsNullOrEmpty(accessToken))
            return Guid.Empty;

        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(accessToken))
            return Guid.Empty;

        var jsonToken = handler.ReadToken(accessToken) as JwtSecurityToken;

        var tokenUserId = jsonToken
            ?.Claims.FirstOrDefault(claim => claim.Type == "UserId")
            ?.Value;

        if (
            string.IsNullOrEmpty(tokenUserId)
            || !Guid.TryParse(tokenUserId, out Guid userId)
        )
            userId = Guid.Empty;

        return userId;
    }

    public string? GetUserEmail()
    {
        var authorizationHeader = _accessor
            .HttpContext!.Request.Headers["Authorization"]
            .ToString();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            return null;

        var accessToken = authorizationHeader["Bearer ".Length..].Trim();

        if (string.IsNullOrEmpty(accessToken))
            return null;

        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(accessToken))
            return null;

        var jsonToken = handler.ReadToken(accessToken) as JwtSecurityToken;

        var userEmail = jsonToken
            ?.Claims.FirstOrDefault(claim => claim.Type == "UserEmail")
            ?.Value;

        if (string.IsNullOrEmpty(userEmail))
            return null;

        return userEmail;
    }

    public string? GetUserName()
    {
        var authorizationHeader = _accessor
            .HttpContext!.Request.Headers["Authorization"]
            .ToString();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            return null;

        var accessToken = authorizationHeader["Bearer ".Length..].Trim();

        if (string.IsNullOrEmpty(accessToken))
            return null;

        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(accessToken))
            return null;

        var jsonToken = handler.ReadToken(accessToken) as JwtSecurityToken;

        var userName = jsonToken
            ?.Claims.FirstOrDefault(claim => claim.Type == "UserName")
            ?.Value;

        if (string.IsNullOrEmpty(userName))
            return null;

        return userName;
    }

    public bool HasValidateUser()
    {
        var endpoint = _accessor.HttpContext!.GetEndpoint();
        
        if (endpoint != null)
        {
            var allowAnonymous = endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>();
            return allowAnonymous == null;
        }

        var path = _accessor.HttpContext.Request.Path.ToString();
        return !path.Contains("/api/account/register") && 
               !path.Contains("/api/account/login") &&
               !path.Contains("/api/v1/Boat");
    }
}
