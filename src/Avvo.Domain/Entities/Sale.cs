using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Interfaces;
using Avvo.Domain.Enums;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Representa uma Venda no domínio de Gestão de Vendas.
    /// </summary>
    public class Sale : BaseEntity, ITenantEntity, IBusinessEntity
    {
        /// <summary>Identificador do Tenant (multitenancy).</summary>
        public Guid TenantId { get; set; }

        /// <summary>Identificador da empresa a qual o cliente pertence.</summary>
        public Guid BusinessId { get; set; }

        public virtual Customer? Customer { get; set; }

        /// <summary>Identificador do cliente.</summary>
        public Guid? CustomerId { get; private set; }
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

        private Sale() { } // ORM

        public Sale(Guid? customerId, Guid? id = null) : base(id)
        {
            CustomerId = customerId;
        }

        /// <summary>
        /// Adiciona um item à venda, referenciando o SKU específico.
        /// </summary>
        public void AddItem(ProductSku sku, decimal quantity, decimal unitPrice, decimal discount = 0)
        {
            if (sku == null) throw new ArgumentNullException(nameof(sku));
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));

            var item = new SaleItem(sku.Id, sku.Product.Name, sku.Variations, quantity, unitPrice, discount);
            _items.Add(item);
        }

        /// <summary>
        /// Adiciona um pagamento à venda.
        /// </summary>
        public void AddPayment(PaymentMethod method, decimal amount)
        {
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
                throw new InvalidOperationException("Não foi possível finalizar a venda, verifique os pagamentos.");

            if (Status != SaleStatus.Open)
                throw new InvalidOperationException("A venda já foi confirmada ou cancelada.");

            Status = SaleStatus.Finalized;
        }

        public void CancelSale()
        {
            if (Status != SaleStatus.Open)
                throw new InvalidOperationException("Only open sales can be canceled.");

            Status = SaleStatus.Canceled;
        }
    }
}
