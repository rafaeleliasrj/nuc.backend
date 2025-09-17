using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Interfaces;
using Avvo.Domain.Enums;

namespace Avvo.Domain.Entities
{
    public class Payment : BaseEntity, IAggregateRoot, ITenantEntity, IBusinessEntity
    {
        /// <summary>Identificador do Tenant (multitenancy).</summary>
        public Guid TenantId { get; set; }

        /// <summary>Identificador da empresa a qual o cliente pertence.</summary>
        public Guid BusinessId { get; set; }
        public PaymentMethod Method { get; }
        public decimal Amount { get; }

        private Payment() { } // ORM

        public Payment(PaymentMethod method, decimal amount)
        {
            Method = method;
            Amount = amount;
        }
    }
}
