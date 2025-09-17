using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Interfaces;

namespace Avvo.Domain.Entities
{
    public class SaleItem : BaseEntity, ITenantEntity, IBusinessEntity
    {
        /// <summary>Identificador do Tenant (multitenancy).</summary>
        public Guid TenantId { get; set; }

        /// <summary>Identificador da empresa a qual o cliente pertence.</summary>
        public Guid BusinessId { get; set; }
        public Guid ProductSkuId { get; private set; }
        public string ProductName { get; private set; }
        public IReadOnlyCollection<ProductVariation> Variations { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Total => (Quantity * UnitPrice) - Discount;

        private SaleItem() { } // ORM

        public SaleItem(
            Guid productSkuId,
            string productName,
            IReadOnlyCollection<ProductVariation> variations,
            decimal quantity,
            decimal unitPrice,
            decimal discount,
            Guid? id = null) : base(id)
        {
            ProductSkuId = productSkuId;
            ProductName = productName;
            Variations = variations;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discount;
        }
    }
}
