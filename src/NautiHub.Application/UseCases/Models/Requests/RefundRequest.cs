namespace NautiHub.Application.UseCases.Models.Requests;

/// <summary>
/// Request para estorno
/// </summary>
public class RefundRequest
{
    /// <summary>
    /// Valor a ser estornado
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Motivo do estorno
    /// </summary>
    public string Reason { get; set; }
}