using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NautiHub.Core.DTOs;
using NautiHub.Core.Interfaces;
using NautiHub.Core.Resources;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace NautiHub.Infrastructure.Services.Identity;

/// <summary>
/// Implementação concreta do serviço de autenticação de tokens
/// </summary>
public class AuthenticationTokenService : IAuthenticationTokenService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationTokenService> _logger;
    private readonly MessagesService _messagesService;

    public AuthenticationTokenService(IConfiguration configuration, ILogger<AuthenticationTokenService> logger, MessagesService messagesService)
    {
        _configuration = configuration;
        _logger = logger;
        _messagesService = messagesService;
    }

    public string GenerateJwtToken(string userId, string email, string userName, IEnumerable<string> roles)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "DefaultSecretKey12345678901234567890");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, userName),
                new Claim("UserId", userId),
                new Claim("UserEmail", email),
                new Claim("UserName", userName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(
                    Convert.ToDouble(_configuration["Jwt:DurationHours"] ?? "24")),
                Issuer = _configuration["Jwt:Issuer"] ?? "NautiHub",
                Audience = _configuration["Jwt:Audience"] ?? "NautiHubUsers",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _messagesService.Token_Generate_Error, userId);
            throw;
        }
    }

    public string GenerateRefreshToken()
    {
        try
        {
            var randomNumber = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _messagesService.Token_Refresh_Error);
            throw;
        }
    }

    public async Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
    {
        // Implementação simplificada - em um ambiente real, 
        // você armazenaria e validaria refresh tokens no banco
        await Task.Delay(1); // Simulação de operação assíncrona
        
        return !string.IsNullOrWhiteSpace(refreshToken) && 
               !string.IsNullOrWhiteSpace(userId) &&
               refreshToken.Length > 20;
    }

    public async Task<TokenClaims?> GetTokenClaimsAsync(string token)
    {
        // Implementação simplificada - em produção você deve adicionar validação real
        if (string.IsNullOrWhiteSpace(token))
            return null;

        return await Task.FromResult(new TokenClaims
        {
            UserId = string.Empty,
            Email = string.Empty,
            UserName = string.Empty,
            Roles = new List<string>(),
            Expiration = DateTime.UtcNow.AddMinutes(30), // Token válido por 30 minutos
            IsExpired = false
        });
    }
}