using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Avvo.Core.Commons.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Avvo.Core.Host.Extensions
{
    public static class JwtTokenConfigurationExtensions
    {

        /// <summary>
        /// Adicionar suporte a autenticação por JWT padrão do Avvo
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="securitySettings">Ver <see cref="JwtSettingsFactory"/> para produzir parâmetros padrões</param>
        /// <returns></returns>
        public static AuthenticationBuilder AddAvvoJwtBearer(this AuthenticationBuilder builder, JwtSettings securitySettings)
        {
            builder.AddJwtBearer(opts => opts.AddAvvoConfiguration(securitySettings));
            return builder;
        }

        /// <summary>
        /// Adicionar suporte a autenticação por JWT padrão do Avvo
        /// Esse método deve ser utilizado quando precisar utilizar mais de um schema de autenticação.
        /// Ex: Permitir Token do Avvo e Token do IdentityServer, Vendr e etc...
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="authenticationScheme">The authentication scheme.</param>
        /// <param name="securitySettings">Ver <see cref="JwtSettingsFactory"/> para produzir parâmetros padrões</param>
        /// <returns></returns>
        public static AuthenticationBuilder AddAvvoJwtBearer(this AuthenticationBuilder builder, string authenticationScheme, JwtSettings securitySettings)
        {
            builder.AddJwtBearer(authenticationScheme, opts => opts.AddAvvoConfiguration(securitySettings));
            return builder;
        }

        /// <summary>
        /// Adicionar suporte a autenticação por JWT padrão do Avvo
        /// </summary>
        /// <param name="bearerOptions"></param>
        /// <param name="securitySettings">Ver <see cref="JwtSettingsFactory"/> para produzir parâmetros padrões</param>
        /// <returns></returns>
        public static JwtBearerOptions AddAvvoConfiguration(this JwtBearerOptions bearerOptions, JwtSettings securitySettings)
        {
            bearerOptions.RequireHttpsMetadata = false;
            bearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
                {
                    return notBefore <= DateTime.UtcNow && expires >= DateTime.UtcNow;
                },
                RequireExpirationTime = false,
                NameClaimType = ClaimTypes.NameIdentifier,
                ValidateAudience = true,
                ValidAudience = securitySettings.Audience,
                ValidateIssuer = true,
                ValidIssuer = securitySettings.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitySettings.SigningKey)),
            };

            bearerOptions.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    if (context.SecurityToken is JwtSecurityToken token && context.Principal.Identity is ClaimsIdentity identity)
                    {
                        identity.AddClaim(new Claim("access_token", token.RawData));
                    }

                    return Task.FromResult(0);
                },

            };

            return bearerOptions;
        }
    }
}
