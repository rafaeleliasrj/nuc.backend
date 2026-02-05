namespace NautiHub.Core.DTOs;

/// <summary>
/// DTO para representar informações do usuário
/// </summary>
public class UserInfoDTO
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLogin { get; set; }
    public Guid? DomainUserId { get; set; }
    public IEnumerable<string> Roles { get; set; } = new List<string>();
    public bool EmailConfirmed { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTime? LockoutEnd { get; set; }
}

/// <summary>
/// DTO para resultado de autenticação
/// </summary>
public class AuthenticationResult
{
    public bool Success { get; set; }
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public UserInfoDTO? User { get; set; }
    public DateTime ExpiresAt { get; set; }
}

/// <summary>
/// DTO para claims extraídos do token
/// </summary>
public class TokenClaims
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public IEnumerable<string> Roles { get; set; } = new List<string>();
    public DateTime Expiration { get; set; }
    public bool IsExpired { get; set; } = DateTime.UtcNow >= DateTime.UtcNow;
}

/// <summary>
/// DTO para registro de usuário
/// </summary>
public class RegisterUserDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// DTO para login de usuário
/// </summary>
public class LoginUserDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; } = false;
}