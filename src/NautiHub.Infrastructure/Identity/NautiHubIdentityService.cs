using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using NautiHub.Core.DomainObjects;

namespace NautiHub.Infrastructure.Identity;

/// <summary>
/// Implementação de INautiHubIdentity que utiliza o UserManager do ASP.NET Core Identity
/// </summary>
public class NautiHubIdentityService : INautiHubIdentity
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid? _requestId;
    private Guid? _userId;
    private string? _userIp;
    private string? _email;
    private string? _name;

    public Guid? RequestId => _requestId;
    public Guid? UserId => _userId;
    public string? UserIp => _userIp;
    public string? Email => _email;
    public string? Name => _name;

    public NautiHubIdentityService(UserManager<UserIdentity> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        
        // Tentar obter informações do usuário atual do contexto HTTP
        LoadCurrentUser();
    }

    private async void LoadCurrentUser()
    {
        try
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            if (user != null)
            {
                _userId = user.Id;
                _email = user.Email;
                // _name = user.FullName; // Comentado devido a ambiguidade - será resolvido depois
            }
        }
        catch
        {
            // Ignorar erros ao carregar usuário
        }
    }

    public void SetRequestId(Guid requestId) => _requestId = requestId;
    public void SetUserId(Guid usuarioId) => _userId = usuarioId;
    public void SetUserIp(string userIp) => _userIp = userIp;
    public void SetEmail(string email) => _email = email;
    public void SetName(string nome) => _name = nome;
}