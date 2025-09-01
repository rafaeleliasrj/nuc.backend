using System;
using Avvo.Domain.Commons;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Representa a Categoria de um Produto no domínio de Gestão de Vendas.
    /// </summary>
    public sealed class ProductCategory : Entity, IAggregateRoot, IEntityTenantControlAccess
    {
        /// <summary>Nome da categoria.</summary>
        public string Name { get; private set; }

        /// <summary>Descrição detalhada da categoria.</summary>
        public string Description { get; private set; }

        private ProductCategory() { } // ORM

        /// <summary>
        /// Cria uma nova Categoria de Produto.
        /// </summary>
        public ProductCategory(Guid? id, string name, string description)
        {
            Id = id ?? Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        /// <summary>
        /// Atualiza os dados da categoria.
        /// </summary>
        public void Update(string name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
}
