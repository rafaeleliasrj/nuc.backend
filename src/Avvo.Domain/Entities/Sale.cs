using System;
using System.Collections.Generic;
using System.Linq;
using Avvo.Core.Abstractions;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Representa uma Venda no domínio de Gestão de Vendas.
    /// </summary>
    public class Sale : BaseEntity, IAggregateRoot, IEntityTenantControlAccess
    {
        public Guid TenantId { get; private set; }
        public Guid? CustomerId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private readonly List<SaleItem> _items = new();
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        private readonly List<Payment> _payments = new();
        public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

        public decimal Discount { get; private set; }
        public decimal Additional { get; private set; }
        public decimal Total => _items.Sum(i => i.Total) - Discount + Additional;
        public decimal Paid => _payments.Sum(p => p.Amount);
        public decimal Change => Paid - Total;

        public SaleStatus Status { get; private set; } = SaleStatus.Open;

        private Sale() { }

        public static Sale Start(Guid tenantId, Guid? customerId = null)
        {
            return new Sale
            {
                TenantId = tenantId,
                CustomerId = customerId
            };
        }

        /// <summary>
        /// Adiciona um item à venda, referenciando o SKU específico.
        /// </summary>
        public void AddItem(ProductSku sku, int quantity, decimal unitPrice, decimal discount = 0)
        {
            if (sku == null) throw new ArgumentNullException(nameof(sku));
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));

            var item = new SaleItem(sku.Id, sku.Product.Name, sku.Variations, quantity, unitPrice, discount);
            _items.Add(item);
        }

        /// <summary>
        /// Adiciona um pagamento à venda.
        /// </summary>
        public void AddPayment(string method, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(method)) throw new ArgumentNullException(nameof(method));
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

            var payment = new Payment(method, amount);
            _payments.Add(payment);
        }

        public void ApplyDiscount(decimal discount) => Discount = discount;
        public void ApplyAdditional(decimal additional) => Additional = additional;

        public bool CanBeFinalized() => _items.Any() && Paid >= Total;

        /// <summary>
        /// Finaliza a venda, disparando evento de domínio.
        /// </summary>
        public void FinalizeSale()
        {
            if (!CanBeFinalized())
                throw new InvalidOperationException("Cannot finalize sale: either no items or insufficient payment.");

            if (Status != SaleStatus.Open)
                throw new InvalidOperationException("A venda já foi confirmada ou cancelada.");

            foreach (var item in _items)
            {
                if (!stockDictionary.TryGetValue(item.ProductSkuId, out var stock))
                    throw new InvalidOperationException($"Não existe estoque cadastrado para o SKU {item.ProductSkuId}.");

                stock.Decrease(
                    item.Quantity,
                    reason: $"Venda {Id}",
                    referenceId: Id
                );
            }

            Status = SaleStatus.Confirmed;

            AddDomainEvent(new SaleConfirmedEvent(Id, TenantId, Total));
        }

        public void CancelSale()
        {
            if (Status != SaleStatus.Open)
                throw new InvalidOperationException("Only open sales can be canceled.");

            foreach (var item in _items)
            {
                if (!stockDictionary.TryGetValue(item.ProductSkuId, out var stock))
                    throw new InvalidOperationException($"Não existe estoque cadastrado para o SKU {item.ProductSkuId}.");

                stock.Increase(
                    item.Quantity,
                    reason: $"Cancelamento da venda {Id}",
                    referenceId: Id
                );
            }

            Status = SaleStatus.Canceled;
            AddDomainEvent(new SaleCanceledEvent(Id, TenantId));
        }
    }

    // Eventos de domínio
    public record SaleConfirmedEvent(Guid SaleId, Guid TenantId, decimal Total) : IDomainEvent;
    public record SaleCanceledEvent(Guid SaleId, Guid TenantId) : IDomainEvent;
}
