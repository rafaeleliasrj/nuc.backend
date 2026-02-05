namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Resposta do comando de criação de pagamento
/// </summary>
public record CreatePaymentCommandResponse
{
    /// <summary>
    /// ID do pagamento
    /// </summary>
    public Guid PaymentId { get; init; }

    /// <summary>
    /// ID do pagamento no Asaas
    /// </summary>
    public string AsaasPaymentId { get; init; }

    /// <summary>
    /// Status do pagamento
    /// </summary>
    public string Status { get; init; }

    /// <summary>
    /// URL da fatura
    /// </summary>
    public string InvoiceUrl { get; init; }

    /// <summary>
    /// URL do boleto
    /// </summary>
    public string BankSlipUrl { get; init; }

    /// <summary>
    /// QR Code do Pix (base64)
    /// </summary>
    public string PixQrCode { get; init; }

    /// <summary>
    /// Payload do Pix
    /// </summary>
    public string PixPayload { get; init; }

    /// <summary>
    /// Data de vencimento
    /// </summary>
    public DateTime? DueDate { get; init; }

    /// <summary>
    /// Valor líquido
    /// </summary>
    public decimal NetValue { get; init; }
}