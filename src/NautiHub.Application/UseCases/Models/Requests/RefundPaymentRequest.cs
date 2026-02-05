namespace NautiHub.Application.UseCases.Models.Requests;

/// <summary>
/// Request para estorno de pagamento
/// </summary>
public class RefundPaymentRequest
{
    /// <summary>
    /// Valor a ser estornado (opcional, se não informado estorna 100%)
    /// </summary>
    public decimal? Value { get; set; }

    /// <summary>
    /// Descrição do motivo do estorno
    /// </summary>
    public string Description { get; set; }
}