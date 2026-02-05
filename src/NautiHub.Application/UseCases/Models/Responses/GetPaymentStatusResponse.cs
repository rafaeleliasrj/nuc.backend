namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response da query de status de pagamento
/// </summary>
public class GetPaymentStatusResponse
{
    /// <summary>
    /// ID do pagamento
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Status do pagamento
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Status legível (em português)
    /// </summary>
    public string StatusDescription { get; set; }

    /// <summary>
    /// Data da última atualização de status
    /// </summary>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    /// Pode ser cancelado?
    /// </summary>
    public bool CanCancel { get; set; }

    /// <summary>
    /// Pode ser estornado?
    /// </summary>
    public bool CanRefund { get; set; }
}