using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Commons.Interfaces;

namespace Avvo.Core.Commons.Entities;

/// <summary>
/// Representa as informações de autenticação para eventos CRUD.
/// </summary>
public class CrudEventAuthentication
{
    /// <summary>
    /// Obtém o token de acesso para autenticação.
    /// </summary>
    public string AccessToken { get; init; }

    /// <summary>
    /// Obtém o token de atualização para autenticação.
    /// </summary>
    public string RefreshToken { get; init; }

    private CrudEventAuthentication(string accessToken, string refreshToken)
    {
        AccessToken = accessToken ?? string.Empty;
        RefreshToken = refreshToken ?? string.Empty;
    }

    /// <summary>
    /// Cria uma instância de <see cref="CrudEventAuthentication"/> a partir de uma identidade de usuário.
    /// </summary>
    /// <param name="userIdentity">A identidade do usuário.</param>
    /// <returns>Um resultado com a instância de <see cref="CrudEventAuthentication"/>.</returns>
    public static CrudEventAuthentication Create(IUserIdentity? userIdentity)
    {
        try
        {
            var auth = new CrudEventAuthentication(
                userIdentity?.AccessToken ?? string.Empty,
                userIdentity?.RefreshAccessToken ?? string.Empty
            );
            return auth;
        }
        catch (Exception ex)
        {
            throw new ServiceException("Erro ao criar autenticação para evento CRUD.", ex);
        }
    }
}
