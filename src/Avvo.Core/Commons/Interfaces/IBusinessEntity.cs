namespace Avvo.Core.Commons.Interfaces;

/// <summary>
/// Define propriedades para entidades associadas a uma empresa sistema Avvo.
/// </summary>
public interface IBusinessEntity
{
    /// <summary>
    /// Obtém o identificador único da empresa.
    /// </summary>
    Guid BusinessId { get; set; }
}
