using System.Text.Json.Serialization;

namespace NautiHub.Infrastructure.Gateways.Asaas.DTOs;

/// <summary>
/// Request para atualizar cliente Asaas
/// </summary>
public class AsaasUpdateCustomerRequest
{
    /// <summary>
    /// Nome do cliente
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Email do cliente
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }

    /// <summary>
    /// Telefone com DDD
    /// </summary>
    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    /// <summary>
    /// Celular com DDD
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
    /// Observações
    /// </summary>
    [JsonPropertyName("observations")]
    public string Observations { get; set; }

    /// <summary>
    /// Indica se o cliente deseja receber notificações por SMS
    /// </summary>
    [JsonPropertyName("notificationDisabled")]
    public bool NotificationDisabled { get; set; }

    /// <summary>
    /// Grupo do cliente
    /// </summary>
    [JsonPropertyName("group")]
    public string Group { get; set; }

    /// <summary>
    /// Dados adicionais
    /// </summary>
    [JsonPropertyName("additionalEmails")]
    public string[] AdditionalEmails { get; set; }

    /// <summary>
    /// Mugshot/Foto
    /// </summary>
    [JsonPropertyName("mugshot")]
    public string Mugshot { get; set; }

    /// <summary>
    /// Grupo para enviar SMS
    /// </summary>
    [JsonPropertyName("groupName")]
    public string GroupName { get; set; }

    /// <summary>
    /// Empresa para envio de SMS
    /// </summary>
    [JsonPropertyName("company")]
    public string Company { get; set; }
}