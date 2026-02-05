using System.Text.Json.Serialization;

namespace NautiHub.Infrastructure.Gateways.Asaas.DTOs;

/// <summary>
/// Request para criar pagamento Asaas
/// </summary>
public class AsaasCreatePaymentRequest
{
    [JsonPropertyName("customer")]
    public string Customer { get; set; }

    [JsonPropertyName("billingType")]
    public string BillingType { get; set; }

    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("externalReference")]
    public string ExternalReference { get; set; }

    [JsonPropertyName("postalService")]
    public bool PostalService { get; set; }

    [JsonPropertyName("split")]
    public List<AsaasDTOs.AsaasPaymentSplit> Split { get; set; }
}

/// <summary>
/// Request para criar pagamento com cartão de crédito
/// </summary>
public class AsaasCreatePaymentWithCreditCardRequest : AsaasCreatePaymentRequest
{
    [JsonPropertyName("creditCard")]
    public AsaasCreditCardToken CreditCard { get; set; }

    [JsonPropertyName("remoteIp")]
    public string RemoteIp { get; set; }

    [JsonPropertyName("creditCardHolderInfo")]
    public AsaasCreditCardHolderInfo HolderInfo { get; set; }
}

/// <summary>
/// Token de cartão de crédito
/// </summary>
public class AsaasCreditCardToken
{
    [JsonPropertyName("creditCardToken")]
    public string CreditCardToken { get; set; }
}

/// <summary>
/// Informações do titular do cartão
/// </summary>
public class AsaasCreditCardHolderInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("cpfCnpj")]
    public string CpfCnpj { get; set; }

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; }

    [JsonPropertyName("addressNumber")]
    public string AddressNumber { get; set; }

    [JsonPropertyName("addressComplement")]
    public string AddressComplement { get; set; }

    [JsonPropertyName("mobilePhone")]
    public string MobilePhone { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("province")]
    public string Province { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }
}

/// <summary>
/// Request para atualizar pagamento
/// </summary>
public class AsaasUpdatePaymentRequest
{
    [JsonPropertyName("billingType")]
    public string BillingType { get; set; }

    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

/// <summary>
/// Status do pagamento
/// </summary>
public class AsaasPaymentStatus
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("reason")]
    public string Reason { get; set; }

    [JsonPropertyName("updateDate")]
    public DateTime UpdateDate { get; set; }
}

/// <summary>
/// Boleto do pagamento
/// </summary>
public class AsaasBankSlip
{
    [JsonPropertyName("bankSlipUrl")]
    public string BankSlipUrl { get; set; }

    [JsonPropertyName("digitableLine")]
    public string DigitableLine { get; set; }

    [JsonPropertyName("barcodeNumber")]
    public string BarcodeNumber { get; set; }

    [JsonPropertyName("nossoNumero")]
    public string NossoNumero { get; set; }
}

/// <summary>
/// QR Code do Pix
/// </summary>
public class AsaasPixQrCode
{
    [JsonPropertyName("encodedImage")]
    public string EncodedImage { get; set; }

    [JsonPropertyName("payload")]
    public string Payload { get; set; }

    [JsonPropertyName("qrCode")]
    public QrCodeData QrCode { get; set; }

    public class QrCodeData
    {
        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}

/// <summary>
/// Request para estorno
/// </summary>
public class AsaasRefundRequest
{
    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

/// <summary>
/// Request para tokenizar cartão
/// </summary>
public class AsaasTokenizeCreditCardRequest
{
    [JsonPropertyName("creditCard")]
    public AsaasCreditCardData CreditCard { get; set; }

    [JsonPropertyName("customer")]
    public string Customer { get; set; }

    [JsonPropertyName("billingType")]
    public string BillingType { get; set; }

    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("remoteIp")]
    public string RemoteIp { get; set; }
}

/// <summary>
/// Dados do cartão para tokenização
/// </summary>
public class AsaasCreditCardData
{
    [JsonPropertyName("holderName")]
    public string HolderName { get; set; }

    [JsonPropertyName("number")]
    public string Number { get; set; }

    [JsonPropertyName("expiryMonth")]
    public int ExpiryMonth { get; set; }

    [JsonPropertyName("expiryYear")]
    public int ExpiryYear { get; set; }

    [JsonPropertyName("cvv")]
    public string Cvv { get; set; }
}

/// <summary>
/// Estorno
/// </summary>
public class AsaasRefund
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; set; }

    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("payment")]
    public string Payment { get; set; }
}