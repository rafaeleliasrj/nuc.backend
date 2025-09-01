using System;

namespace Avvo.Core.Commons.Jwt;

/// <summary>
/// Define as configurações de segurança para tokens JWT.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Obtém a chave de assinatura do token JWT.
    /// </summary>
    public required string SigningKey { get; init; }

    /// <summary>
    /// Obtém o emissor do token JWT.
    /// </summary>
    public required string Issuer { get; init; }

    /// <summary>
    /// Obtém o público-alvo do token JWT.
    /// </summary>
    public required string Audience { get; init; }

    public JwtSettings()
    {
    }

    public JwtSettings(string signingKey, string issuer, string audience)
    {
        SigningKey = ValidateNonEmpty(signingKey, nameof(signingKey));
        Issuer = ValidateNonEmpty(issuer, nameof(issuer));
        Audience = ValidateNonEmpty(audience, nameof(audience));
    }

    private static string ValidateNonEmpty(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"O valor de {paramName} não pode ser nulo ou vazio.", paramName);
        return value;
    }
}