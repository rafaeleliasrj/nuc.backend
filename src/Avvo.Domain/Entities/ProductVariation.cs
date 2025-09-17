using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Interfaces;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Representa uma variação de um produto (ex: cor, tamanho).
    /// </summary>
    public class ProductVariation : BaseEntity, ITenantEntity, IBusinessEntity
    {
        /// <summary>Identificador do Tenant (multitenancy).</summary>
        public Guid TenantId { get; set; }

        /// <summary>Identificador da empresa a qual o cliente pertence.</summary>
        public Guid BusinessId { get; set; }

        public virtual ProductSku ProductSku { get; set; }

        public Guid ProductSkuId { get; set; }
        public string Type { get; private set; }
        public string Value { get; private set; }

        private ProductVariation() { } // ORM

        public ProductVariation(string type, string value, Guid? id = null) : base(id)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void Update(string type, string value)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
