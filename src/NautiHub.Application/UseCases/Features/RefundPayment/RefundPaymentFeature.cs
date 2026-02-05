using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.RefundPayment;

/// <summary>
/// Feature para estornar pagamento
/// </summary>
public class RefundPaymentFeature : Feature<FeatureResponse<RefundPaymentResponse>>
{
    /// <summary>
    /// ID do pagamento
    /// </summary>
    public Guid PaymentId { get; set; }

    /// <summary>
    /// Valor a ser estornado
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Motivo do estorno
    /// </summary>
    public string Reason { get; set; }
}