using NautiHub.Core.DTOs;

namespace NautiHub.Core.Interfaces;

/// <summary>
/// Interface para serviço de geração e validação de tokens de autenticação
/// </summary>
public interface IAuthenticationTokenService
{
    /// <summary>
    /// Gera token JWT para o usuário
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <param name="email">Email do usuário</param>
    /// <param name="userName">Nome de usuário</param>
    /// <param name="roles">Roles do usuário</param>
    /// <returns>Token JWT</returns>
    string GenerateJwtToken(string userId, string email, string userName, IEnumerable<string> roles);

    /// <summary>
    /// Gera refresh token
    /// </summary>
    /// <returns>Refresh token</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Valida refresh token
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <param name="refreshToken">Refresh token</param>
    /// <returns>True se válido</returns>
    Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken);

    /// <summary>
    /// Obtém claims do token JWT
    /// </summary>
    /// <param name="token">Token JWT</param>
    /// <returns>Claims do usuário</returns>
    Task<TokenClaims?> GetTokenClaimsAsync(string token);
}