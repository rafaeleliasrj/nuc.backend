using System.Text.Json.Serialization;

namespace NautiHub.Infrastructure.Gateways.Asaas.DTOs;

/// <summary>
/// Informações do cartão de crédito do Asaas
/// </summary>
public class AsaasCreditCard
{
    /// <summary>
    /// Número do cartão (mascarado)
    /// </summary>
    [JsonPropertyName("creditCardNumber")]
    public string CreditCardNumber { get; set; }

    /// <summary>
    /// Bandeira do cartão
    /// </summary>
    [JsonPropertyName("creditCardBrand")]
    public string CreditCardBrand { get; set; }

    /// <summary>
    /// Token do cartão
    /// </summary>
    [JsonPropertyName("creditCardToken")]
    public string CreditCardToken { get; set; }
}