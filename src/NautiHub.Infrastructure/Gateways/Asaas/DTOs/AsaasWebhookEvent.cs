using System.Text.Json.Serialization;

namespace NautiHub.Infrastructure.Gateways.Asaas.DTOs;

/// <summary>
/// Evento de webhook do Asaas
/// </summary>
public class AsaasWebhookEvent
{
    /// <summary>
    /// Tipo do evento
    /// </summary>
    [JsonPropertyName("event")]
    public string Event { get; set; }

    /// <summary>
    /// ID do evento
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Dados do pagamento (se aplicável)
    /// </summary>
    [JsonPropertyName("payment")]
    public AsaasPayment Payment { get; set; }

    /// <summary>
    /// Dados da cobrança (se aplicável)
    /// </summary>
    [JsonPropertyName("invoice")]
    public object Invoice { get; set; }

    /// <summary>
    /// Dados do split (se aplicável)
    /// </summary>
    [JsonPropertyName("split")]
    public AsaasDTOs.AsaasPaymentSplit Split { get; set; }

    /// <summary>
    /// Data da criação do evento
    /// </summary>
    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; set; }
}