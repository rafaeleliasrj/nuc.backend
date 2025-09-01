using System;
using System.Collections.Generic;
using Avvo.GestaoVenda.Domain.Commons;

namespace Avvo.GestaoVenda.Domain.Entities
{
    /// <summary>
    /// Representa um Produto no domínio de Gestão de Vendas.
    /// Produto é um Aggregate Root e possui SKUs para cada combinação de variações.
    /// </summary>
    public sealed class Product : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public UnitOfMeasure UnitOfMeasure { get; private set; }
        public ProductCategory Category { get; private set; }

        /// <summary>Lista de SKUs do produto, cada SKU representa uma variação vendável.</summary>
        public IReadOnlyCollection<ProductSku> Skus => _skus.AsReadOnly();
        private readonly List<ProductSku> _skus = new();

        public bool IsActive { get; private set; }

        private Product() { } // ORM

        public Product(Guid? id, string name, string description, UnitOfMeasure unitOfMeasure, ProductCategory category)
        {
            Id = id ?? Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            UnitOfMeasure = unitOfMeasure ?? throw new ArgumentNullException(nameof(unitOfMeasure));
            Category = category ?? throw new ArgumentNullException(nameof(category));
            IsActive = true;
        }

        /// <summary>Atualiza os dados principais do produto.</summary>
        public void Update(string name, string description, UnitOfMeasure unitOfMeasure, ProductCategory category)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            UnitOfMeasure = unitOfMeasure ?? throw new ArgumentNullException(nameof(unitOfMeasure));
            Category = category ?? throw new ArgumentNullException(nameof(category));
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
