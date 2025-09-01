namespace Avvo.Core.Commons.Interfaces;

/// <summary>
/// Define a configuração para propagação de eventos CRUD.
/// </summary>
public interface ICrudEventProcessor
{
    /// <summary>
    /// Obtém ou define se o evento CRUD deve ser propagado.
    /// Usa a variável de ambiente EVENT_CRUD_PROCESS_API para configurar o endpoint.
    /// </summary>
    bool PropagateEvent { get; set; }

    /// <summary>
    /// Obtém ou define a lista de IDs de destinos para os quais o evento deve ser propagado.
    /// </summary>
    IList<int>? PropagateDestination { get; set; }

    /// <summary>
    /// Obtém ou define o nome da entidade a ser sobrescrito no evento.
    /// </summary>
    string? OverrideEntityName { get; set; }

    /// <summary>
    /// Obtém ou define os dados personalizados do evento.
    /// </summary>
    IDictionary<string, string>? EventCustomData { get; set; }
}
