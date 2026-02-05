namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response da feature de estorno de pagamento
/// </summary>
public class RefundPaymentResponse
{
    /// <summary>
    /// ID do pagamento
    /// </summary>
    public Guid PaymentId { get; set; }

    /// <summary>
    /// ID do estorno
    /// </summary>
    public string RefundId { get; set; }

    /// <summary>
    /// Valor estornado
    /// </summary>
    public decimal RefundedValue { get; set; }

    /// <summary>
    /// Data do estorno
    /// </summary>
    public DateTime RefundDate { get; set; }

    /// <summary>
    /// Status do estorno
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Motivo do estorno
    /// </summary>
    public string Reason { get; set; }

    /// <summary>
    /// Status atual do pagamento
    /// </summary>
    public string PaymentStatus { get; set; }
}