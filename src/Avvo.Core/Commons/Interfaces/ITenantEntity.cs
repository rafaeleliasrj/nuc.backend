using System;

namespace Avvo.Core.Commons.Interfaces;

/// <summary>
/// Define propriedades para entidades associadas a um tenant (assinatura) no sistema Avvo.
/// </summary>
public interface ITenantEntity
{
    /// <summary>
    /// Obtém o identificador único do tenant (assinatura).
    /// </summary>
    Guid TenantId { get; set; }
}
