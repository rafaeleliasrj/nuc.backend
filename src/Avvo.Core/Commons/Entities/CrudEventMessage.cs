using System.Net;
using Avvo.Core.Commons.Exceptions;

namespace Avvo.Core.Commons.Entities;

/// <summary>
/// Representa uma mensagem de evento CRUD.
/// </summary>
public class CrudEventMessage
{
    /// <summary>
    /// Obtém o identificador único do evento.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Obtém o identificador da entidade associada ao evento.
    /// </summary>
    public Guid EntityId { get; init; }

    /// <summary>
    /// Obtém o identificador do tenant (assinatura).
    /// </summary>
    public Guid TenantId { get; init; }

    /// <summary>
    /// Obtém a operação CRUD executada.
    /// </summary>
    public string Operation { get; init; }

    /// <summary>
    /// Obtém o tipo da entidade.
    /// </summary>
    public string EntityType { get; init; }

    /// <summary>
    /// Obtém os dados da entidade.
    /// </summary>
    public object Data { get; init; }

    /// <summary>
    /// Obtém as informações de autenticação do evento.
    /// </summary>
    public CrudEventAuthentication Authentication { get; init; }

    /// <summary>
    /// Obtém a lista de destinos para propagação do evento.
    /// </summary>
    public IList<int> Destinations { get; init; }

    /// <summary>
    /// Obtém os dados personalizados do evento.
    /// </summary>
    public IDictionary<string, string>? CustomData { get; init; }

    private CrudEventMessage(
        Guid id,
        Guid entityId,
        Guid tenantId,
        string operation,
        string entityType,
        object data,
        CrudEventAuthentication authentication,
        IList<int> destinations,
        IDictionary<string, string>? customData)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O ID do evento não pode ser vazio.", nameof(id));
        if (entityId == Guid.Empty)
            throw new ArgumentException("O ID da entidade não pode ser vazio.", nameof(entityId));
        if (tenantId == Guid.Empty)
            throw new ArgumentException("O ID do tenant não pode ser vazio.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(operation))
            throw new ArgumentException("A operação não pode ser nula ou vazia.", nameof(operation));
        if (string.IsNullOrWhiteSpace(entityType))
            throw new ArgumentException("O tipo da entidade não pode ser nulo ou vazio.", nameof(entityType));
        if (authentication == null)
            throw new ArgumentNullException(nameof(authentication));
        if (destinations == null)
            throw new ArgumentNullException(nameof(destinations));
        if (customData == null)
            throw new ArgumentNullException(nameof(customData));

        Id = id;
        EntityId = entityId;
        TenantId = tenantId;
        Operation = operation;
        EntityType = entityType.EndsWith("Proxy") ? entityType.Replace("Proxy", "") : entityType;
        Data = data;
        Authentication = authentication;
        Destinations = destinations;
        CustomData = customData;
    }

    /// <summary>
    /// Cria uma instância de <see cref="CrudEventMessage"/>.
    /// </summary>
    /// <param name="entityId">O ID da entidade.</param>
    /// <param name="tenantId">O ID do tenant.</param>
    /// <param name="operation">A operação CRUD.</param>
    /// <param name="entityType">O tipo da entidade.</param>
    /// <param name="data">Os dados da entidade.</param>
    /// <param name="authentication">As informações de autenticação.</param>
    /// <param name="destinations">Os destinos de propagação.</param>
    /// <param name="customData">Os dados personalizados.</param>
    /// <returns>Uma instância de <see cref="CrudEventMessage"/>.</returns>
    public static CrudEventMessage Create(
        Guid entityId,
        Guid tenantId,
        CrudEventOperationEnum operation,
        string entityType,
        object data,
        CrudEventAuthentication authentication,
        IList<int> destinations,
        IDictionary<string, string> customData)
    {
        try
        {
            var message = new CrudEventMessage(
                Guid.NewGuid(),
                entityId,
                tenantId,
                operation.ToString(),
                entityType,
                data,
                authentication,
                destinations,
                customData
            );
            return message;
        }
        catch (ArgumentException ex)
        {
            throw new ServiceException("Erro ao criar mensagem de evento CRUD.", ex);
        }
    }
}
