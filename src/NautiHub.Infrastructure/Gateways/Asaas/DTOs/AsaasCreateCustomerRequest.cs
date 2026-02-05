using System.Text.Json.Serialization;

namespace NautiHub.Infrastructure.Gateways.Asaas.DTOs;

/// <summary>
/// Cliente Asaas para criação
/// </summary>
public class AsaasCreateCustomerRequest
{
    /// <summary>
    /// Nome do cliente (Obrigatório)
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// CPF (11 dígitos) ou CNPJ (14 dígitos) (Obrigatório)
    /// </summary>
    [JsonPropertyName("cpfCnpj")]
    public string CpfCnpj { get; set; }

    /// <summary>
    /// Email do cliente (Obrigatório)
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }

    /// <summary>
    /// Telefone com DDD (Opcional)
    /// </summary>
    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    /// <summary>
    /// Celular com DDD (Opcional)
    /// </summary>
    [JsonPropertyName("mobilePhone")]
    public string MobilePhone { get; set; }

    /// <summary>
    /// Endereço (Opcional)
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; }

    /// <summary>
    /// Número (Opcional)
    /// </summary>
    [JsonPropertyName("addressNumber")]
    public string AddressNumber { get; set; }

    /// <summary>
    /// Complemento (Opcional)
    /// </summary>
    [JsonPropertyName("complement")]
    public string Complement { get; set; }

    /// <summary>
    /// Bairro (Opcional)
    /// </summary>
    [JsonPropertyName("province")]
    public string Province { get; set; }

    /// <summary>
    /// Cidade (Opcional)
    /// </summary>
    [JsonPropertyName("city")]
    public string City { get; set; }

    /// <summary>
    /// Estado (Opcional)
    /// </summary>
    [JsonPropertyName("state")]
    public string State { get; set; }

    /// <summary>
    /// CEP (Opcional)
    /// </summary>
    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; }

    /// <summary>
    /// País (Opcional)
    /// </summary>
    [JsonPropertyName("country")]
    public string Country { get; set; }

    /// <summary>
    /// Observações (Opcional)
    /// </summary>
    [JsonPropertyName("observations")]
    public string Observations { get; set; }

    /// <summary>
    /// Indica se o cliente deseja receber notificações por SMS (Opcional)
    /// </summary>
    [JsonPropertyName("notificationDisabled")]
    public bool NotificationDisabled { get; set; }

    /// <summary>
    /// Grupo do cliente (Opcional)
    /// </summary>
    [JsonPropertyName("group")]
    public string Group { get; set; }

    /// <summary>
    /// Identificador externo (Opcional)
    /// </summary>
    [JsonPropertyName("externalReference")]
    public string ExternalReference { get; set; }

    /// <summary>
    /// Dados adicionais (Opcional)
    /// </summary>
    [JsonPropertyName("additionalEmails")]
    public string[] AdditionalEmails { get; set; }

    /// <summary>
    /// Mugshot/Foto (Opcional)
    /// </summary>
    [JsonPropertyName("mugshot")]
    public string Mugshot { get; set; }

    /// <summary>
    /// Grupo para enviar SMS (Opcional)
    /// </summary>
    [JsonPropertyName("groupName")]
    public string GroupName { get; set; }

    /// <summary>
    /// Empresa para envio de SMS (Opcional)
    /// </summary>
    [JsonPropertyName("company")]
    public string Company { get; set; }
}