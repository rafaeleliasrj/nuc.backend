using System.Net;
using Avvo.Core.Commons.Exceptions;

namespace Avvo.Core.Commons.Jwt;

public static class JwtSettingsFactory
{
    public static JwtSettings Create(string issuer, string audience, string signingKey)
    {
        try
        {
            var settings = new JwtSettings { Audience = audience, Issuer = issuer, SigningKey = signingKey };
            return settings;
        }
        catch (ArgumentException ex)
        {
            throw new HttpStatusException(HttpStatusCode.BadRequest, ex.Message, "E400");
        }
    }

    public static JwtSettings CreateFromEnvironmentVariables(
        string issuerEnvName = "AUTHENTICATION_ISSUER",
        string audienceEnvName = "AUTHENTICATION_AUDIENCE",
        string signingKeyEnvName = "AUTHENTICATION_SIGNING_KEY")
    {
        try
        {
            var issuer = Environment.GetEnvironmentVariable(issuerEnvName);
            var audience = Environment.GetEnvironmentVariable(audienceEnvName);
            var signingKey = Environment.GetEnvironmentVariable(signingKeyEnvName);

            if (string.IsNullOrWhiteSpace(issuer))
                throw new HttpStatusException(HttpStatusCode.BadRequest, $"A variável de ambiente {issuerEnvName} não está definida.", "E400");
            if (string.IsNullOrWhiteSpace(audience))
                throw new HttpStatusException(HttpStatusCode.BadRequest, $"A variável de ambiente {audienceEnvName} não está definida.", "E400");
            if (string.IsNullOrWhiteSpace(signingKey))
                throw new HttpStatusException(HttpStatusCode.BadRequest, $"A variável de ambiente {signingKeyEnvName} não está definida.", "E400");

            var settings = new JwtSettings { Audience = audience, Issuer = issuer, SigningKey = signingKey };
            return settings;
        }
        catch (HttpStatusException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ServiceException("Erro ao criar configurações JWT a partir de variáveis de ambiente.", ex);
        }
    }
}
