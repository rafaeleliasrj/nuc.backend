using Avvo.Core.Commons.Entities;

namespace Avvo.Domain.Entities
{
    public class Address : BaseEntity
    {
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }
        public string Country { get; private set; }

        private Address() { } // ORM

        public Address(
            string street,
            string number,
            string neighborhood,
            string city,
            string state,
            string zipCode,
            string country)
        {
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            ZipCode = zipCode;
            Country = country;
        }
    }
}
