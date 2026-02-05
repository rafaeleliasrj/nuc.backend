using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.PaymentUpdate;

/// <summary>
/// Feature para atualização de pagamento
/// </summary>
public class UpdatePaymentFeature : Feature<FeatureResponse<PaymentResponse>>
{
    /// <summary>
    /// Identificador do pagamento
    /// </summary>
    public Guid PaymentId { get; init; }

    /// <summary>
    /// Request de atualização de pagamento
    /// </summary>
    public UpdatePaymentRequest Data { get; init; }
}