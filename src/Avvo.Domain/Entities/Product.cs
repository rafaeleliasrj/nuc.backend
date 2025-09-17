using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Interfaces;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Representa um Produto no domínio de Gestão de Vendas.
    /// Produto é um Aggregate Root e possui SKUs para cada combinação de variações.
    /// </summary>
    public class Product : BaseEntity, IAggregateRoot, ITenantEntity, IBusinessEntity
    {
        /// <summary>Identificador do Tenant (multitenancy).</summary>
        public Guid TenantId { get; set; }

        /// <summary>Identificador da empresa a qual o cliente pertence.</summary>
        public Guid BusinessId { get; set; }

        /// <summary>Nome do produto.</summary>
        public string Name { get; private set; }

        /// <summary>Nome curto do produto.</summary>
        public string? ShortName { get; private set; }

        /// <summary>Descrição do produto.</summary>
        public string? Description { get; private set; }
        public UnitOfMeasure UnitOfMeasure { get; private set; }
        public ProductCategory? Category { get; private set; }

        /// <summary>Lista de SKUs do produto, cada SKU representa uma variação vendável.</summary>
        public IReadOnlyCollection<ProductSku> Skus => _skus.AsReadOnly();
        private readonly List<ProductSku> _skus = new();

        public bool IsActive { get; private set; }

        private Product() { } // ORM

        public Product(string name, string? shortName, string? description, UnitOfMeasure unitOfMeasure, ProductCategory? category, Guid? id = null) : base(id)
        {
            Name = name;
            ShortName = shortName;
            Description = description;
            UnitOfMeasure = unitOfMeasure;
            Category = category;
            IsActive = true;
        }

        /// <summary>Atualiza os dados principais do produto.</summary>
        public void Update(string name, string? shortName, string? description, UnitOfMeasure unitOfMeasure, ProductCategory? category)
        {
            Name = name;
            ShortName = shortName;
            Description = description;
            UnitOfMeasure = unitOfMeasure;
            Category = category;
        }

        /// <summary>Adiciona um SKU ao produto.</summary>
        public void AddSku(ProductSku sku)
        {
            if (sku == null) throw new ArgumentNullException(nameof(sku));
            _skus.Add(sku);
        }

        /// <summary>Remove um SKU do produto.</summary>
        public void RemoveSku(ProductSku sku)
        {
            if (sku == null) throw new ArgumentNullException(nameof(sku));
            _skus.Remove(sku);
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}
