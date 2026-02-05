using Microsoft.AspNetCore.Http;
using NautiHub.Core.DomainObjects;

namespace NautiHub.Infrastructure.Identity;

/// <summary>
/// Implementação simples de INautiHubIdentity que não causa dependência circular
/// </summary>
public class SimpleNautiHubIdentity : INautiHubIdentity
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid? _requestId;

    public Guid? RequestId => _requestId;
    public Guid? UserId => GetCurrentUserId();
    public string? UserIp => GetUserIp();
    public string? Email => GetUserEmail();
    public string? Name => GetUserName();

    public SimpleNautiHubIdentity(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private Guid? GetCurrentUserId()
    {
        try
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : null;
        }
        catch
        {
            return null;
        }
    }

    private string? GetUserEmail() => _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

    private string? GetUserName() => _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ??
                   _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value;

    private string? GetUserIp() => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

    public void SetRequestId(Guid requestId) => _requestId = requestId;
    public void SetUserId(Guid userId) { /* Implementado se necessário */ }
    public void SetUserIp(string userIp) { /* Implementado se necessário */ }
    public void SetEmail(string email) { /* Implementado se necessário */ }
    public void SetName(string name) { /* Implementado se necessário */ }
}