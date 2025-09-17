using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Interfaces;
using Avvo.Domain.Enums;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Estoque de um SKU de produto.
    /// Responsável por controlar a quantidade disponível e gerar movimentações.
    /// </summary>
    public class Stock : BaseEntity, IAggregateRoot, ITenantEntity, IBusinessEntity
    {
        /// <summary>Identificador do Tenant (multitenancy).</summary>
        public Guid TenantId { get; set; }

        /// <summary>Identificador da empresa a qual o cliente pertence.</summary>
        public Guid BusinessId { get; set; }
        public Guid ProductSkuId { get; private set; }
        public decimal Quantity { get; private set; }

        private readonly List<StockMovement> _movements = new();
        public IReadOnlyCollection<StockMovement> Movements => _movements.AsReadOnly();

        private Stock() { } // ORM

        public Stock(Guid productSkuId, decimal initialQuantity = 0, Guid? id = null) : base(id)
        {
            ProductSkuId = productSkuId;
            Quantity = initialQuantity;
        }

        public void Increase(int quantity, string reason, Guid? referenceId = null)
        {
            if (quantity <= 0)
                throw new InvalidOperationException("A quantidade deve ser positiva.");

            Quantity += quantity;

            var movement = new StockMovement(
                MovementType.Entry,
                quantity,
                reason,
                referenceId
            );

            _movements.Add(movement);
        }

        public void Decrease(int quantity, string reason, Guid? referenceId = null)
        {
            if (quantity <= 0)
                throw new InvalidOperationException("A quantidade deve ser positiva.");

            if (Quantity < quantity)
                throw new InvalidOperationException("Estoque insuficiente.");

            Quantity -= quantity;

            var movement = new StockMovement(
                MovementType.Exit,
                quantity,
                reason,
                referenceId
            );

            _movements.Add(movement);
        }

        /// <summary>
        /// Ajuste de estoque (balanço). Seta a quantidade física real contada em inventário.
        /// </summary>
        public void Balance(decimal newQuantity, string reason, Guid? referenceId = null)
        {
            if (newQuantity < 0)
                throw new InvalidOperationException("Estoque não pode ser negativo.");

            var difference = newQuantity - Quantity;

            Quantity = newQuantity;

            var movement = new StockMovement(
                MovementType.Balance,
                difference,
                reason,
                referenceId
            );

            _movements.Add(movement);
        }
    }
}
