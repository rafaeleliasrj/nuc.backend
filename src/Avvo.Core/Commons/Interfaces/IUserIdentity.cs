namespace Avvo.Core.Commons.Interfaces;

/// <summary>
/// Define propriedades para a identidade do usuário no sistema Avvo.
/// </summary>
public interface IUserIdentity
{
    /// <summary>
    /// Obtém o identificador único do usuário.
    /// </summary>
    Guid UserId { get; set; }

    /// <summary>
    /// Obtém o endereço de e-mail do usuário.
    /// </summary>
    string Email { get; set; }

    /// <summary>
    /// Obtém o nome do usuário.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Obtém o identificador único da assinatura (tenant) do usuário.
    /// </summary>
    Guid TenantId { get; set; }

    /// <summary>
    /// Obtém o identificador único da empresa.
    /// </summary>
    Guid BusinessId { get; set; }

    /// <summary>
    /// Obtém a coleção de escopos atribuídos ao usuário.
    /// </summary>
    ICollection<string> Scopes { get; set; }

    /// <summary>
    /// Obtém o token de acesso do usuário.
    /// </summary>
    string AccessToken { get; set; }

    /// <summary>
    /// Obtém o token de atualização do usuário.
    /// </summary>
    string RefreshAccessToken { get; set; }
}
