using Avvo.Core.Abstractions;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Estoque de um SKU de produto.
    /// Responsável por controlar a quantidade disponível e gerar movimentações.
    /// </summary>
    public class Stock : Entity, IAggregateRoot, IEntityTenantControlAccess
    {
        public Guid TenantId { get; private set; }
        public Guid ProductSkuId { get; private set; }
        public int Quantity { get; private set; }

        private readonly List<StockMovement> _movements = new();
        public IReadOnlyCollection<StockMovement> Movements => _movements.AsReadOnly();

        private Stock() { }

        public static Stock Create(Guid tenantId, Guid productSkuId, int initialQuantity = 0)
        {
            return new Stock
            {
                TenantId = tenantId,
                ProductSkuId = productSkuId,
                Quantity = initialQuantity
            };
        }

        public void Increase(int quantity, string reason, Guid? referenceId = null)
        {
            if (quantity <= 0)
                throw new InvalidOperationException("A quantidade deve ser positiva.");

            Quantity += quantity;

            var movement = new StockMovement(
                Guid.NewGuid(),
                DateTime.UtcNow,
                MovementType.Entry,
                quantity,
                reason,
                referenceId
            );

            _movements.Add(movement);

            AddDomainEvent(new StockIncreasedEvent(Id, ProductSkuId, quantity));
        }

        public void Decrease(int quantity, string reason, Guid? referenceId = null)
        {
            if (quantity <= 0)
                throw new InvalidOperationException("A quantidade deve ser positiva.");

            if (Quantity < quantity)
                throw new InvalidOperationException("Estoque insuficiente.");

            Quantity -= quantity;

            var movement = new StockMovement(
                Guid.NewGuid(),
                DateTime.UtcNow,
                MovementType.Exit,
                quantity,
                reason,
                referenceId
            );

            _movements.Add(movement);

            AddDomainEvent(new StockDecreasedEvent(Id, ProductSkuId, quantity));
        }

        /// <summary>
        /// Ajuste de estoque (balanço). Seta a quantidade física real contada em inventário.
        /// </summary>
        public void Balance(int newQuantity, string reason, Guid? referenceId = null)
        {
            if (newQuantity < 0)
                throw new InvalidOperationException("Estoque não pode ser negativo.");

            var difference = newQuantity - Quantity;

            Quantity = newQuantity;

            var movement = new StockMovement(
                Guid.NewGuid(),
                DateTime.UtcNow,
                MovementType.Balance,
                difference,
                reason,
                referenceId
            );

            _movements.Add(movement);

            AddDomainEvent(new StockBalancedEvent(Id, ProductSkuId, newQuantity, difference));
        }
    }

    // Eventos de domínio
    public record StockIncreasedEvent(Guid StockId, Guid ProductSkuId, int Quantity) : IDomainEvent;
    public record StockDecreasedEvent(Guid StockId, Guid ProductSkuId, int Quantity) : IDomainEvent;
    public record StockBalancedEvent(Guid StockId, Guid ProductSkuId, int NewQuantity, int Difference) : IDomainEvent;
}
