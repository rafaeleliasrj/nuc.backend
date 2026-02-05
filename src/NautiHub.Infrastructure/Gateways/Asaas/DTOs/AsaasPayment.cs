using System.Text.Json.Serialization;
using NautiHub.Infrastructure.Gateways.Asaas.DTOs;

namespace NautiHub.Infrastructure.Gateways.Asaas.DTOs;

/// <summary>
/// Pagamento Asaas
/// </summary>
public class AsaasPayment
{
    /// <summary>
    /// ID do pagamento
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Tipo do objeto (sempre "payment")
    /// </summary>
    [JsonPropertyName("object")]
    public string Object { get; set; }

    /// <summary>
    /// ID do cliente
    /// </summary>
    [JsonPropertyName("customer")]
    public string Customer { get; set; }

    /// <summary>
    /// Data de criação
    /// </summary>
    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// Data de vencimento
    /// </summary>
    [JsonPropertyName("dueDate")]
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Data de vencimento original
    /// </summary>
    [JsonPropertyName("originalDueDate")]
    public DateTime OriginalDueDate { get; set; }

    /// <summary>
    /// Valor do pagamento
    /// </summary>
    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    /// <summary>
    /// Valor líquido (após taxas)
    /// </summary>
    [JsonPropertyName("netValue")]
    public decimal NetValue { get; set; }

    /// <summary>
    /// Valor original (quando diferente do valor pago)
    /// </summary>
    [JsonPropertyName("originalValue")]
    public decimal? OriginalValue { get; set; }

    /// <summary>
    /// Valor de juros
    /// </summary>
    [JsonPropertyName("interestValue")]
    public decimal? InterestValue { get; set; }

    /// <summary>
    /// Nosso número (boleto)
    /// </summary>
    [JsonPropertyName("nossoNumero")]
    public string NossoNumero { get; set; }

    /// <summary>
    /// Descrição
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// Referência externa
    /// </summary>
    [JsonPropertyName("externalReference")]
    public string ExternalReference { get; set; }

    /// <summary>
    /// Tipo de cobrança (BOLETO, CREDIT_CARD, PIX, etc.)
    /// </summary>
    [JsonPropertyName("billingType")]
    public string BillingType { get; set; }

    /// <summary>
    /// Status do pagamento
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    /// <summary>
    /// Transação Pix
    /// </summary>
    [JsonPropertyName("pixTransaction")]
    public object PixTransaction { get; set; }

    /// <summary>
    /// Data de confirmação
    /// </summary>
    [JsonPropertyName("confirmedDate")]
    public DateTime? ConfirmedDate { get; set; }

    /// <summary>
    /// Data do pagamento
    /// </summary>
    [JsonPropertyName("paymentDate")]
    public DateTime? PaymentDate { get; set; }

    /// <summary>
    /// Data do pagamento pelo cliente
    /// </summary>
    [JsonPropertyName("clientPaymentDate")]
    public DateTime? ClientPaymentDate { get; set; }

    /// <summary>
    /// Número da parcela
    /// </summary>
    [JsonPropertyName("installmentNumber")]
    public int? InstallmentNumber { get; set; }

    /// <summary>
    /// Data de crédito
    /// </summary>
    [JsonPropertyName("creditDate")]
    public DateTime? CreditDate { get; set; }

    /// <summary>
    /// Valor em custódia
    /// </summary>
    [JsonPropertyName("custody")]
    public object Custody { get; set; }

    /// <summary>
    /// Data estimada de crédito
    /// </summary>
    [JsonPropertyName("estimatedCreditDate")]
    public DateTime? EstimatedCreditDate { get; set; }

    /// <summary>
    /// URL da fatura
    /// </summary>
    [JsonPropertyName("invoiceUrl")]
    public string InvoiceUrl { get; set; }

    /// <summary>
    /// URL do boleto
    /// </summary>
    [JsonPropertyName("bankSlipUrl")]
    public string BankSlipUrl { get; set; }

    /// <summary>
    /// URL do comprovante
    /// </summary>
    [JsonPropertyName("transactionReceiptUrl")]
    public string TransactionReceiptUrl { get; set; }

    /// <summary>
    /// Número da fatura
    /// </summary>
    [JsonPropertyName("invoiceNumber")]
    public string InvoiceNumber { get; set; }

    /// <summary>
    /// Indica se foi deletado
    /// </summary>
    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }

    /// <summary>
    /// Indica se foi antecipado
    /// </summary>
    [JsonPropertyName("anticipated")]
    public bool Anticipated { get; set; }

    /// <summary>
    /// Indica se é antecipável
    /// </summary>
    [JsonPropertyName("anticipable")]
    public bool Anticipable { get; set; }

    /// <summary>
    /// Data da última visualização da fatura
    /// </summary>
    [JsonPropertyName("lastInvoiceViewedDate")]
    public DateTime? LastInvoiceViewedDate { get; set; }

    /// <summary>
    /// Data da última visualização do boleto
    /// </summary>
    [JsonPropertyName("lastBankSlipViewedDate")]
    public DateTime? LastBankSlipViewedDate { get; set; }

    /// <summary>
    /// Indica se é serviço postal
    /// </summary>
    [JsonPropertyName("postalService")]
    public bool PostalService { get; set; }

    /// <summary>
    /// Informações do cartão de crédito
    /// </summary>
    [JsonPropertyName("creditCard")]
    public AsaasCreditCard CreditCard { get; set; }

    /// <summary>
    /// Desconto
    /// </summary>
    [JsonPropertyName("discount")]
    public AsaasDTOs.Discount Discount { get; set; }

    /// <summary>
    /// Multa
    /// </summary>
    [JsonPropertyName("fine")]
    public AsaasDTOs.Fine Fine { get; set; }

    /// <summary>
    /// Juros
    /// </summary>
    [JsonPropertyName("interest")]
    public AsaasDTOs.Interest Interest { get; set; }

    /// <summary>
    /// Splits de pagamento
    /// </summary>
    [JsonPropertyName("split")]
    public List<AsaasDTOs.AsaasPaymentSplit> Split { get; set; }

    /// <summary>
    /// Chargeback
    /// </summary>
    [JsonPropertyName("chargeback")]
    public AsaasDTOs.Chargeback Chargeback { get; set; }

    /// <summary>
    /// Estornos
    /// </summary>
    [JsonPropertyName("refunds")]
    public List<object> Refunds { get; set; }
}