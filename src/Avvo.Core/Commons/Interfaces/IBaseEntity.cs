using System;

namespace Avvo.Core.Commons.Interfaces;

/// <summary>
/// Define as propriedades básicas para entidades no sistema Avvo.
/// </summary>
public interface IBaseEntity
{
    /// <summary>
    /// Obtém o identificador único da entidade.
    /// </summary>
    Guid Id { get; set; }

    /// <summary>
    /// Obtém ou define a data e hora da última atualização da entidade (UTC).
    /// </summary>
    DateTimeOffset? UpdateDate { get; set; }

    /// <summary>
    /// Obtém a data e hora de criação da entidade (UTC).
    /// </summary>
    DateTimeOffset? CreateDate { get; set; }

    /// <summary>
    /// Obtém ou define o ID do usuário que atualizou a entidade pela última vez.
    /// </summary>
    Guid? UpdateUserId { get; set; }

    /// <summary>
    /// Obtém o ID do usuário que criou a entidade.
    /// </summary>
    Guid? CreateUserId { get; set; }
}
