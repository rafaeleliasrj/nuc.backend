using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NautiHub.Infrastructure.Gateways.Asaas.DTOs;

/// <summary>
/// DTOs auxiliares do Asaas
/// </summary>
public static class AsaasDTOs
{
    public class Discount
    {
        [JsonPropertyName("value")]
        public decimal Value { get; set; }

        [JsonPropertyName("dueDateLimitDays")]
        public int DueDateLimitDays { get; set; }

        [JsonPropertyName("limitedDate")]
        public DateTime? LimitedDate { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Fine
    {
        [JsonPropertyName("value")]
        public decimal Value { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Interest
    {
        [JsonPropertyName("value")]
        public decimal Value { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class AsaasPaymentSplit
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("walletId")]
        public string WalletId { get; set; }

        [JsonPropertyName("fixedValue")]
        public decimal FixedValue { get; set; }

        [JsonPropertyName("percentualValue")]
        public decimal PercentualValue { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("refusalReason")]
        public string RefusalReason { get; set; }

        [JsonPropertyName("externalReference")]
        public string ExternalReference { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class Chargeback
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }
    }
}

