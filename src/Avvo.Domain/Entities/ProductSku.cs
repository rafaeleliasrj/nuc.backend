namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Representa um SKU (Stock Keeping Unit) de um produto.
    /// Cada SKU corresponde a uma combinação de variações ou um produto sem variação.
    /// </summary>
    public sealed class ProductSku : Entity, IAggregateRoot
    {
        public Product Product { get; private set; }
        public IReadOnlyCollection<ProductVariation> Variations => _variations.AsReadOnly();
        private readonly List<ProductVariation> _variations = new();
        public decimal Price { get; private set; }
        public int Stock { get; private set; }

        public ProductSku(Guid? id, Product product, IEnumerable<ProductVariation> variations, decimal price, int stock)
        {
            Id = id ?? Guid.NewGuid();
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
