using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Interfaces;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Representa uma Unidade de Medida (ex: kg, litro, unidade) no domínio de Gestão de Vendas.
    /// </summary>
    public sealed class UnitOfMeasure : BaseEntity, IAggregateRoot, ITenantEntity, IBusinessEntity
    {
        /// <summary>Identificador do Tenant (multitenancy).</summary>
        public Guid TenantId { get; set; }

        /// <summary>Identificador da empresa a qual o cliente pertence.</summary>
        public Guid BusinessId { get; set; }

        /// <summary>Sigla da unidade de medida (ex: kg, L, un).</summary>
        public string Code { get; private set; }

        /// <summary>Descrição completa da unidade de medida.</summary>
        public string Description { get; private set; }

        private UnitOfMeasure() { } // ORM

        /// <summary>
        /// Cria uma nova Unidade de Medida.
        /// </summary>
        public UnitOfMeasure(Guid? id, string code, string description)
        {
            Id = id ?? Guid.NewGuid();
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        /// <summary>
        /// Atualiza os dados da unidade de medida.
        /// </summary>
        public void Update(string code, string description)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
}
