using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.PaymentDelete;

/// <summary>
/// Feature para exclusão de pagamento
/// </summary>
public class DeletePaymentFeature : Feature<FeatureResponse<bool>>
{
    /// <summary>
    /// Identificador do pagamento
    /// </summary>
    public Guid PaymentId { get; init; }
}