using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Extensions;
using Avvo.Core.Commons.Interfaces;
using Avvo.Domain.Enums;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Representa a entidade Empresa dentro do domínio de Gestão de Vendas.
    /// </summary>
    public sealed class Business : BaseEntity, IAggregateRoot, ITenantEntity
    {
        /// <summary>
        /// Identificador do Tenant (multitenancy).
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// Nome fantasia da empresa.
        /// </summary>
        public string TradeName { get; private set; }

        /// <summary>
        /// Razão social da empresa.
        /// </summary>
        public string CorporateName { get; private set; }

        /// <summary>
        /// CNAE da empresa.
        /// </summary>
        public Cnae? Cnae { get; private set; }

        /// <summary>
        /// Porte da empresa.
        /// </summary>
        public CompanySize? CompanySize { get; set; }

        /// <summary>
        /// Regime especial de tributação da empresa.
        /// </summary>
        public SpecialTaxRegime? SpecialTaxRegime { get; private set; }

        /// <summary>
        /// Código do Regime tributário da empresa.
        /// </summary>
        public TaxRegimeCode? TaxRegimeCode { get; private set; }

        /// <summary>
        /// Opção de Simples Nacional da empresa.
        /// </summary>
        public SimplesNationalOption? SimplesNationalOption { get; private set; }

        /// <summary>
        /// Regime de apuração de Simples Nacional da empresa.
        /// </summary>
        public SimplesNationalTaxCalculationRegime? SimplesNationalTaxCalculationRegime { get; private set; }


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
        public BusinessAddress Address { get; private set; }

        /// <summary>
        /// E-mail da empresa.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Telefone da empresa.
        /// </summary>
        public string Phone { get; private set; }

        /// <summary>
        /// Celular da empresa.
        /// </summary>
        public string Mobile { get; private set; }


        private Business() { } // ORM

        /// <summary>
        /// Cria uma nova instância da entidade Empresa.
        /// </summary>
        public Business(
            string tradeName,
            string corporateName,
            string cnpj,
            string? stateRegistration,
            string? municipalRegistration,
            string email,
            string phone,
            string mobile,
            BusinessAddress address,
            Cnae cnae,
            CompanySize companySize,
            SpecialTaxRegime specialTaxRegime,
            TaxRegimeCode taxRegimeCode,
            SimplesNationalOption? simplesNationalOption = null,
            SimplesNationalTaxCalculationRegime? simplesNationalTaxCalculationRegime = null,
            Guid? id = null
        ) : base(id)
        {
            Cnpj = cnpj.OnlyNumbers();
            Update(tradeName,
                   corporateName,
                   stateRegistration,
                   municipalRegistration,
                   email,
                   phone,
                   mobile,
                   address,
                   cnae,
                   companySize,
                   specialTaxRegime,
                   taxRegimeCode,
                   simplesNationalOption,
                   simplesNationalTaxCalculationRegime);
        }

        /// <summary>
        /// Atualiza os dados principais da empresa.
        /// </summary>
        public void Update(string tradeName,
            string corporateName,
            string? stateRegistration,
            string? municipalRegistration,
            string email,
            string phone,
            string mobile,
            BusinessAddress address,
            Cnae cnae,
            CompanySize companySize,
            SpecialTaxRegime specialTaxRegime,
            TaxRegimeCode taxRegimeCode,
            SimplesNationalOption? simplesNationalOption = null,
            SimplesNationalTaxCalculationRegime? simplesNationalTaxCalculationRegime = null)
        {
            TradeName = tradeName;
            CorporateName = corporateName;
            StateRegistration = stateRegistration;
            MunicipalRegistration = municipalRegistration;
            Email = email;
            Phone = phone;
            Mobile = mobile;
            Address = address;
            Cnae = cnae;
            CompanySize = companySize;
            SpecialTaxRegime = specialTaxRegime;
            TaxRegimeCode = taxRegimeCode;
            SimplesNationalOption = simplesNationalOption;
            SimplesNationalTaxCalculationRegime = simplesNationalTaxCalculationRegime;
        }
    }
}
