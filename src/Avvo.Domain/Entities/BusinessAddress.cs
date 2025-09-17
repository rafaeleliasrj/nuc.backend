using Avvo.Core.Commons.Interfaces;

namespace Avvo.Domain.Entities
{
    public class BusinessAddress : Address, ITenantEntity
    {
        /// <summary>
        /// Identificador do Tenant (multitenancy).
        /// </summary>
        public Guid TenantId { get; set; }

        public Guid BusinessId { get; private set; }

        public virtual Business Business { get; set; }

        public BusinessAddress(
            string street,
            string number,
            string neighborhood,
            string city,
            string state,
            string zipCode,
            string country)
            : base(street, number, neighborhood, city, state, zipCode, country)
        {
        }
    }
}
