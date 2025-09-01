namespace Avvo.Core.Commons.Entities;

/// <summary>
/// Define os tipos de operações CRUD para processamento de eventos.
/// </summary>
public enum CrudEventOperationEnum
{
    /// <summary>
    /// Operação de criação de uma entidade.
    /// </summary>
    Create,

    /// <summary>
    /// Operação de atualização de uma entidade.
    /// </summary>
    Update,

    /// <summary>
    /// Operação de exclusão de uma entidade.
    /// </summary>
    Delete,

    /// <summary>
    /// Operação de exclusão suave de uma entidade.
    /// </summary>
    SoftDelete
}