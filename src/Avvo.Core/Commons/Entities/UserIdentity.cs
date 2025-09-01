using Avvo.Core.Commons.Interfaces;

namespace Avvo.Core.Commons.Entities;

/// <summary>
/// Representa a identidade de um usuário no sistema Avvo.
/// </summary>
public class UserIdentity : IUserIdentity
{
    /// <summary>
    /// Obtém o identificador único do usuário.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Obtém o endereço de e-mail do usuário.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Obtém o nome do usuário.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Obtém o identificador único da assinatura (tenant) do usuário.
    /// </summary>
    public Guid SubscriptionId { get; set; }

    /// <summary>
    /// Obtém a coleção de escopos atribuídos ao usuário.
    /// </summary>
    public ICollection<string> Scopes { get; set; }

    /// <summary>
    /// Obtém o token de acesso do usuário.
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Obtém o token de atualização do usuário.
    /// </summary>
    public string RefreshAccessToken { get; set; }
}
