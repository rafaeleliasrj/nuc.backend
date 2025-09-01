using Avvo.Core.Commons.Jwt;

namespace Avvo.Core.Services.Jwt
{
    public interface ICreateJwtService
    {
        /// <summary>
        /// Criar token JWT com parâmetros de sessão e segurança informados
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="secutiryParameters"></param>
        /// <returns></returns>
        string Execute(JwtSessionParameters parameters, JwtSettings secutiryParameters);

        /// <summary>
        /// Criar token JWT com parâmetros de sessão indicados e utilizar a instância de segurança padrão <see cref="JwtSecuritySettings"/> configurada na injeção de dependência da aplicação.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string Execute(JwtSessionParameters parameters);
    }
}
