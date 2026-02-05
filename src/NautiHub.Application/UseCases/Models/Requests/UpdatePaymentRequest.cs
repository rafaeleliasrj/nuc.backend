namespace NautiHub.Application.UseCases.Models.Requests;

/// <summary>
/// Request para atualização de pagamento
/// </summary>
public class UpdatePaymentRequest
{
    /// <summary>
    /// Descrição atualizada do pagamento
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Data de vencimento atualizada
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Referência externa atualizada
    /// </summary>
    public string ExternalReference { get; set; }
}