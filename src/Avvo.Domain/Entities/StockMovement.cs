using Avvo.Core.Abstractions;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Movimentações de estoque são imutáveis.
    /// </summary>
    public record StockMovement(
        Guid Id,
        DateTime Date,
        MovementType Type,
        int Quantity,
        string Reason,
        Guid? ReferenceId
    );
}
