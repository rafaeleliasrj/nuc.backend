using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Interfaces;
using Avvo.Domain.Enums;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Movimentações de estoque são imutáveis.
    /// </summary>
    public class StockMovement : BaseEntity, ITenantEntity, IBusinessEntity
    {
        /// <summary>Identificador do Tenant (multitenancy).</summary>
        public Guid TenantId { get; set; }

        /// <summary>Identificador da empresa a qual o cliente pertence.</summary>
        public Guid BusinessId { get; set; }
        public MovementType Type { get; private set; }
        public decimal Quantity { get; private set; }
        public string Reason { get; private set; }
        public Guid? ReferenceId { get; private set; }

        private StockMovement() { } // ORM

        public StockMovement(
            MovementType type,
            decimal quantity,
            string reason,
            Guid? referenceId,
            Guid? id = null) : base(id)
        {
            Type = type;
            Quantity = quantity;
            Reason = reason;
            ReferenceId = referenceId;
            Id = id ?? Guid.NewGuid();
        }
    }
}
