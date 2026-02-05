namespace NautiHub.Application.UseCases.Models.Requests;

/// <summary>
/// Dados do cartão de crédito
/// </summary>
public class CreditCardRequest
{
    /// <summary>
    /// Nome do titular
    /// </summary>
    public string HolderName { get; set; }

    /// <summary>
    /// Número do cartão
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Mês de vencimento
    /// </summary>
    public int ExpiryMonth { get; set; }

    /// <summary>
    /// Ano de vencimento
    /// </summary>
    public int ExpiryYear { get; set; }

    /// <summary>
    /// Código de segurança (3 dígitos)
    /// </summary>
    public string Cvv { get; set; }

    /// <summary>
    /// CPF/CNPJ do titular
    /// </summary>
    public string CpfCnpj { get; set; }

    /// <summary>
    /// Email do titular
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// CEP do titular
    /// </summary>
    public string PostalCode { get; set; }

    /// <summary>
    /// Endereço do titular
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Número do endereço
    /// </summary>
    public string AddressNumber { get; set; }

    /// <summary>
    /// Complemento do endereço
    /// </summary>
    public string AddressComplement { get; set; }

    /// <summary>
    /// Bairro do titular
    /// </summary>
    public string Province { get; set; }

    /// <summary>
    /// Cidade do titular
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// Estado do titular
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// País do titular
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Telefone móvel do titular
    /// </summary>
    public string MobilePhone { get; set; }

    /// <summary>
    /// IP do cliente (anti-fraude)
    /// </summary>
    public string RemoteIp { get; set; }
}