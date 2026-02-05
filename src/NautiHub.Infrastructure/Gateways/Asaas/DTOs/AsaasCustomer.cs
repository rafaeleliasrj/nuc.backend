using System.Text.Json.Serialization;

namespace NautiHub.Infrastructure.Gateways.Asaas.DTOs;

/// <summary>
/// Cliente Asaas
/// </summary>
public class AsaasCustomer
{
    /// <summary>
    /// ID do cliente
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Nome do cliente
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// CPF/CNPJ
    /// </summary>
    [JsonPropertyName("cpfCnpj")]
    public string CpfCnpj { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }

    /// <summary>
    /// Telefone
    /// </summary>
    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    /// <summary>
    /// Celular
    /// </summary>
    [JsonPropertyName("mobilePhone")]
    public string MobilePhone { get; set; }

    /// <summary>
    /// Endereço
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; }

    /// <summary>
    /// Número
    /// </summary>
    [JsonPropertyName("addressNumber")]
    public string AddressNumber { get; set; }

    /// <summary>
    /// Complemento
    /// </summary>
    [JsonPropertyName("complement")]
    public string Complement { get; set; }

    /// <summary>
    /// Bairro
    /// </summary>
    [JsonPropertyName("province")]
    public string Province { get; set; }

    /// <summary>
    /// Cidade
    /// </summary>
    [JsonPropertyName("city")]
    public string City { get; set; }

    /// <summary>
    /// Estado
    /// </summary>
    [JsonPropertyName("state")]
    public string State { get; set; }

    /// <summary>
    /// CEP
    /// </summary>
    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; }

    /// <summary>
    /// País
    /// </summary>
    [JsonPropertyName("country")]
    public string Country { get; set; }

    /// <summary>
    /// Identificador externo
    /// </summary>
    [JsonPropertyName("externalReference")]
    public string ExternalReference { get; set; }

    /// <summary>
    /// Data de criação
    /// </summary>
    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// Indica se notificação está desabilitada
    /// </summary>
    [JsonPropertyName("notificationDisabled")]
    public bool NotificationDisabled { get; set; }

    /// <summary>
    /// Grupo
    /// </summary>
    [JsonPropertyName("group")]
    public string Group { get; set; }

    /// <summary>
    /// Observações
    /// </summary>
    [JsonPropertyName("observations")]
    public string Observations { get; set; }

    /// <summary>
    /// Indica se foi deletado
    /// </summary>
    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
}