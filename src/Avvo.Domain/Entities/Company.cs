using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Avvo.Domain.Commons;
using Avvo.Domain.Enums;
using Avvo.Domain.Events.Companies;
using Avvo.Domain.ValueObjects;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Representa a entidade Empresa dentro do domínio de Gestão de Vendas.
    /// </summary>
    public sealed class Company : Entity, IAggregateRoot, IEntityTenantControlAccess
    {
        /// <summary>
        /// Identificador do Tenant (multitenancy).
        /// </summary>
        public Guid TenantId { get; private set; }

        /// <summary>
        /// Nome fantasia da empresa.
        /// </summary>
        public string TradeName { get; private set; }

        /// <summary>
        /// Razão social da empresa.
        /// </summary>
        public string CorporateName { get; private set; }

        /// <summary>
        /// Documento CNPJ da empresa.
        /// </summary>
        public string Cnpj { get; private set; }

        /// <summary>
        /// Inscrição estadual da empresa.
        /// </summary>
        public string? StateRegistration { get; private set; }

        /// <summary>
        /// Inscrição municipal da empresa.
        /// </summary>
        public string? MunicipalRegistration { get; private set; }

        /// <summary>
        /// Endereço da empresa.
        /// </summary>
        public Address Address { get; private set; }

        /// <summary>
        /// Informações de contato da empresa.
        /// </summary>
        public ContactInfo ContactInfo { get; private set; }

        /// <summary>
        /// Regime tributário da empresa.
        /// </summary>
        public TaxRegime TaxRegime { get; private set; }

        /// <summary>
        /// Código do regime tributário da empresa (para integração).
        /// </summary>
        public TaxRegimeCode TaxRegimeCode => (TaxRegimeCode)((int)TaxRegime + 1);

        private Company() { } // ORM

        /// <summary>
        /// Cria uma nova instância da entidade Empresa.
        /// </summary>
        public Company(Guid? id = null,
                       Guid tenantId = default,
                       string tradeName = "",
                       string corporateName = "",
                       string cnpj = "",
                       string? stateRegistration = null,
                       string? municipalRegistration = null,
                       Address? address = null,
                       ContactInfo? contactInfo = null,
                       TaxRegime taxRegime = TaxRegime.SimplesNacional)
        {
            Id = id ?? Guid.NewGuid();
            TenantId = tenantId != Guid.Empty ? tenantId : throw new ArgumentNullException(nameof(tenantId));
            TradeName = tradeName;
            CorporateName = corporateName;
            Cnpj = NormalizeCnpj(cnpj);
            StateRegistration = stateRegistration;
            MunicipalRegistration = municipalRegistration;
            Address = address ?? Address.Empty;
            ContactInfo = contactInfo ?? ContactInfo.Empty;
            TaxRegime = taxRegime;

            AddDomainEvent(new CompanyCreatedEvent(this));
        }

        /// <summary>
        /// Atualiza os dados principais da empresa.
        /// </summary>
        public void Update(string tradeName,
                           string corporateName,
                           string cnpj,
                           string? stateRegistration,
                           string? municipalRegistration,
                           Address address,
                           ContactInfo contactInfo,
                           TaxRegime taxRegime)
        {
            TradeName = tradeName;
            CorporateName = corporateName;
            Cnpj = NormalizeCnpj(cnpj);
            StateRegistration = stateRegistration;
            MunicipalRegistration = municipalRegistration;
            Address = address;
            ContactInfo = contactInfo;
            TaxRegime = taxRegime;

            AddDomainEvent(new CompanyUpdatedEvent(this));
        }

        private static string NormalizeCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj)) return string.Empty;
            return Regex.Replace(cnpj, "\\D", "");
        }
    }
}
