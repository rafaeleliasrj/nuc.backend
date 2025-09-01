using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Avvo.Core.Commons.Jwt;

namespace Avvo.Core.Services.Jwt
{
    public class CreateJwtService : ICreateJwtService
    {
        private readonly JwtSettings _jwtSecuritySettings;


        public CreateJwtService(JwtSettings jwtSecuritySettings)
        {
            _jwtSecuritySettings = jwtSecuritySettings;
        }

        public string Execute(JwtSessionParameters sessionParameters, JwtSettings secutiryParameters)
        {
            var claims = new List<Claim>
            {
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ("N")),
                new Claim (System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName, sessionParameters.Username),
                new Claim (System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, sessionParameters.UserId.ToString()),
            };

            if (sessionParameters.CustomClaims != null && sessionParameters.CustomClaims.Any())
                foreach (var item in sessionParameters.CustomClaims)
                    claims.Add(new Claim(item.Key, item.Value));

            var identity = new ClaimsIdentity(
                new GenericIdentity(sessionParameters.UserId.ToString(), "identity_id"),
                claims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secutiryParameters.SigningKey));
            var creeds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = secutiryParameters.Issuer,
                Audience = secutiryParameters.Audience,
                SigningCredentials = creeds,
                Subject = identity,
                NotBefore = sessionParameters.CreatedDate,
                Expires = sessionParameters.ExpirationDate,
                IssuedAt = sessionParameters.CreatedDate
            });

            var token = handler.WriteToken(securityToken);
            return token;
        }

        public string Execute(JwtSessionParameters parameters) => Execute(parameters, _jwtSecuritySettings);
    }
}
