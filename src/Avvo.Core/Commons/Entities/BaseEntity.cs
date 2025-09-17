using Avvo.Core.Commons.Interfaces;
using Avvo.Core.Commons.Utils;

namespace Avvo.Core.Commons.Entities;

/// <summary>
/// Define as propriedades básicas para entidades no sistema Avvo.
/// </summary>
public class BaseEntity(Guid? id = null) : IBaseEntity, ISoftDelete
{
    /// <summary>
    /// Obtém o identificador único da entidade.
    /// </summary>
    public Guid Id { get; set; } = id == null ? SequentialGuidGenerator.NewSequentialGuid() : id.Value;

    /// <summary>
    /// Obtém ou define a data e hora da última atualização da entidade (UTC).
    /// </summary>
    public DateTimeOffset? UpdateDate { get; set; }

    /// <summary>
    /// Obtém a data e hora de criação da entidade (UTC).
    /// </summary>
    public DateTimeOffset? CreateDate { get; set; }

    /// <summary>
    /// Obtém ou define o ID do usuário que atualizou a entidade pela última vez.
    /// </summary>
    public Guid? UpdateUserId { get; set; }

    /// <summary>
    /// Obtém o ID do usuário que criou a entidade.
    /// </summary>
    public Guid? CreateUserId { get; set; }

    /// <summary>
    /// Indica se a entidade foi excluida.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Obtém a data e hora da exclusão da entidade (UTC).
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Obtém o ID do usuário que excluiu a entidade.
    /// </summary>
    public Guid? DeletedUserId { get; set; }

    public void Undo()
    {
        throw new NotImplementedException();
    }
}
