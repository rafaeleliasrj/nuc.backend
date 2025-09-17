using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Extensions;
using Avvo.Core.Commons.Interfaces;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Representa um cliente dentro do domínio de Gestão de Vendas.
    /// </summary>
    public sealed class Customer : BaseEntity, IAggregateRoot, ITenantEntity, IBusinessEntity
    {
        /// <summary>Identificador do Tenant (multitenancy).</summary>
        public Guid TenantId { get; set; }

        /// <summary>Identificador da empresa a qual o cliente pertence.</summary>
        public Guid BusinessId { get; set; }

        /// <summary>Nome completo do cliente.</summary>
        public string? FullName { get; private set; }

        /// <summary>CPF ou CNPJ do cliente.</summary>
        public string Document { get; private set; }

        /// <summary>Email de contato do cliente.</summary>
        public string? Email { get; private set; }

        /// <summary>Telefone principal do cliente.</summary>
        public string? Phone { get; private set; }

        /// <summary>Endereço principal do cliente.</summary>
        public Address? Address { get; private set; }

        /// <summary>Indicador se o cliente está ativo no sistema.</summary>
        public bool IsActive { get; private set; }

        private Customer() { } // ORM

        /// <summary>
        /// Cria uma nova instância do cliente só com o documento.
        /// </summary>
        public Customer(string document, Guid? id = null) : base(id)
        {
            Document = document.OnlyNumbers();
            IsActive = true;
        }

        /// <summary>
        /// Cria uma nova instância do cliente.
        /// </summary>
        public Customer(string fullName,
                        string document,
                        string email,
                        string phone,
                        Address address,
                        Guid? id = null) : base(id)
        {
            FullName = fullName;
            Document = document.OnlyNumbers();
            Email = email;
            Phone = phone.OnlyNumbers();
            Address = address;
            IsActive = true;
        }

        /// <summary>
        /// Atualiza as informações do cliente.
        /// </summary>
        public void Update(string fullName, string document, string email, string phone, Address address)
        {
            FullName = fullName;
            Document = document.OnlyNumbers();
            Email = email;
            Phone = phone.OnlyNumbers();
            Address = address;
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
