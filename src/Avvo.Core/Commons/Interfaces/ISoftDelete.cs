using System;

namespace Avvo.Core.Commons.Interfaces;

/// <summary>
/// Define propriedades e métodos para entidades que suportam exclusão suave.
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// Obtém ou define se a entidade está marcada como excluída.
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Obtém ou define a data e hora em que a entidade foi excluída (UTC).
    /// </summary>
    DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Obtém ou define o ID do usuário que excluiu a entidade.
    /// </summary>
    Guid? DeletedUserId { get; set; }

    /// <summary>
    /// Reverte a exclusão suave, limpando os marcadores de exclusão.
    /// </summary>
    void Undo();
}
