using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Interfaces;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Representa um SKU (Stock Keeping Unit) de um produto.
    /// Cada SKU corresponde a uma combinação de variações ou um produto sem variação.
    /// </summary>
    public class ProductSku : BaseEntity, ITenantEntity, IBusinessEntity
    {
        /// <summary>Identificador do Tenant (multitenancy).</summary>
        public Guid TenantId { get; set; }

        /// <summary>Identificador da empresa a qual o cliente pertence.</summary>
        public Guid BusinessId { get; set; }

        /// <summary>Identificador do produto ao qual o SKU pertence.</summary>
        public virtual Product Product { get; private set; }

        /// <summary>Identificador do SKU.</summary>
        public Guid ProductId { get; private set; }

        /// <summary>Variações do SKU.</summary>
        public IReadOnlyCollection<ProductVariation> Variations => _variations.AsReadOnly();
        private readonly List<ProductVariation> _variations = new();

        /// <summary>Preço do SKU.</summary>
        public decimal Price { get; private set; }

        /// <summary>Quantidade em estoque do SKU.</summary>
        public decimal Stock { get; private set; }

        private ProductSku() { } // ORM

        public ProductSku(Product product, IEnumerable<ProductVariation> variations, decimal price, decimal stock, Guid? id = null) : base(id)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));
            _variations.AddRange(variations ?? throw new ArgumentNullException(nameof(variations)));
            Price = price;
            Stock = stock;
        }

        public void UpdateStock(int quantity)
        {
            Stock += quantity;
        }

        public void UpdatePrice(decimal price)
        {
            Price = price;
        }
    }
}
