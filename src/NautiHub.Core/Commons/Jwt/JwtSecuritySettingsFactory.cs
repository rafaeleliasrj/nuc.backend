using NautiHub.Core.Utils;

namespace NautiHub.Core.Commons.Jwt;

public static class JwtSecuritySettingsFactory
{
    /// <summary>
    /// Configurar <see cref="JwtSecuritySettings"/> com os valores indicados nos argumentos.
    /// </summary>
    /// <param name="issuer"></param>
    /// <param name="audience"></param>
    /// <param name="signingKey"></param>
    public static JwtSecuritySettings Create(string issuer, string audience, string signingKey) =>
        new()
        {
            Issuer = issuer,
            Audience = audience,
            SigningKey = signingKey,
        };

    /// <summary>
    /// Configurar <see cref="JwtSecuritySettings" /> com variáveis de ambiente padrões do framework: AUTHENTICATION_ISSUER, AUTHENTICATION_AUDIENCE e AUTHENTICATION_SIGNING_KEY
    /// </summary>
    /// <param name="issuerEnvName"></param>
    /// <param name="audienceEnvName"></param>
    /// <param name="signingKeyEnvName"></param>
    public static JwtSecuritySettings CreateFromEnvironmentVariables(
        string issuerEnvName = "AUTHENTICATION_ISSUER",
        string audienceEnvName = "AUTHENTICATION_AUDIENCE",
        string signingKeyEnvName = "AUTHENTICATION_SIGNING_KEY"
    ) =>
        new()
        {
            Issuer = EnvironmentVariables.Get(issuerEnvName),
            Audience = EnvironmentVariables.Get(audienceEnvName),
            SigningKey = EnvironmentVariables.Get(signingKeyEnvName),
        };
}
