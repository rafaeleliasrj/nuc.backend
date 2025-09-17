using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Interfaces;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Representa a Categoria de um Produto no domínio de Gestão de Vendas.
    /// </summary>
    public sealed class ProductCategory : BaseEntity, IAggregateRoot, ITenantEntity, IBusinessEntity
    {
        /// <summary>Identificador do Tenant (multitenancy).</summary>
        public Guid TenantId { get; set; }

        /// <summary>Identificador da empresa a qual o cliente pertence.</summary>
        public Guid BusinessId { get; set; }

        /// <summary>Nome da categoria.</summary>
        public string Name { get; private set; }

        /// <summary>Descrição detalhada da categoria.</summary>
        public string? Description { get; private set; }

        private ProductCategory() { } // ORM

        /// <summary>
        /// Cria uma nova Categoria de Produto.
        /// </summary>
        public ProductCategory(string name, string? description, Guid? id = null) : base(id)
        {
            Name = name;
            Description = description;
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
