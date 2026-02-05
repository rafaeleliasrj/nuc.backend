using NautiHub.Domain.ValueObjects;
using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response com dados do pagamento para API.
/// </summary>
public class PaymentResponse
{
    /// <summary>
    /// Identificador único do pagamento.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID do pagamento no gateway Asaas.
    /// </summary>
    public string AsaasPaymentId { get; set; } = string.Empty;

    /// <summary>
    /// ID da reserva associada ao pagamento.
    /// </summary>
    public Guid BookingId { get; set; }

    /// <summary>
    /// Valor bruto do pagamento.
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Valor líquido do pagamento (após taxas).
    /// </summary>
    public decimal NetValue { get; set; }

    /// <summary>
    /// Método de pagamento utilizado.
    /// </summary>
    public PaymentMethod Method { get; set; }

    /// <summary>
    /// Status do pagamento.
    /// </summary>
    public PaymentStatus Status { get; set; }

    /// <summary>
    /// URL da invoice/fatura.
    /// </summary>
    public string InvoiceUrl { get; set; } = string.Empty;

    /// <summary>
    /// URL do boleto bancário.
    /// </summary>
    public string BankSlipUrl { get; set; } = string.Empty;

    /// <summary>
    /// URL do comprovante de transação.
    /// </summary>
    public string TransactionReceiptUrl { get; set; } = string.Empty;

    /// <summary>
    /// QR Code para pagamento Pix.
    /// </summary>
    public string PixQrCode { get; set; } = string.Empty;

    /// <summary>
    /// Imagem codificada do QR Code Pix.
    /// </summary>
    public string PixEncodedImage { get; set; } = string.Empty;

    /// <summary>
    /// Data de vencimento do pagamento.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Data de confirmação do pagamento.
    /// </summary>
    public DateTime? ConfirmedDate { get; set; }

    /// <summary>
    /// Data do pagamento efetivo.
    /// </summary>
    public DateTime? PaymentDate { get; set; }

    /// <summary>
    /// Data de crédito na conta.
    /// </summary>
    public DateTime? CreditDate { get; set; }

    /// <summary>
    /// Descrição do pagamento.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Referência externa do pagamento.
    /// </summary>
    public string ExternalReference { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de cobrança.
    /// </summary>
    public string BillingType { get; set; } = string.Empty;

    /// <summary>
    /// Informações mascaradas do cartão de crédito.
    /// </summary>
    public CreditCardInfo? CreditCardInfo { get; set; }

    /// <summary>
    /// Lista de divisões do pagamento.
    /// </summary>
    public List<PaymentSplit> Splits { get; set; } = new();

    /// <summary>
    /// Data de criação do pagamento.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data da última atualização.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}