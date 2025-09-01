using System;
using Avvo.Domain.Commons;
using Avvo.Domain.ValueObjects;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Representa um cliente dentro do domínio de Gestão de Vendas.
    /// </summary>
    public sealed class Customer : Entity, IAggregateRoot, IEntityTenantControlAccess
    {
        /// <summary>Nome completo do cliente.</summary>
        public string FullName { get; private set; }

        /// <summary>CPF ou CNPJ do cliente.</summary>
        public string Document { get; private set; }

        /// <summary>Email de contato do cliente.</summary>
        public string Email { get; private set; }

        /// <summary>Telefone principal do cliente.</summary>
        public string Phone { get; private set; }

        /// <summary>Endereço principal do cliente.</summary>
        public Address Address { get; private set; }

        /// <summary>Indicador se o cliente está ativo no sistema.</summary>
        public bool IsActive { get; private set; }

        private Customer() { } // ORM

        /// <summary>
        /// Cria uma nova instância do cliente.
        /// </summary>
        public Customer(Guid? id,
                        string fullName,
                        string document,
                        string email,
                        string phone,
                        Address address)
        {
            Id = id ?? Guid.NewGuid();
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Document = document ?? throw new ArgumentNullException(nameof(document));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            IsActive = true;
        }

        /// <summary>
        /// Atualiza as informações do cliente.
        /// </summary>
        public void Update(string fullName, string document, string email, string phone, Address address)
        {
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Document = document ?? throw new ArgumentNullException(nameof(document));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        /// <summary>
        /// Ativa o cliente.
        /// </summary>
        public void Activate() => IsActive = true;

        /// <summary>
        /// Desativa o cliente.
        /// </summary>
        public void Deactivate() => IsActive = false;
    }
}
