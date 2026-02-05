using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.GenerateBoleto;

/// <summary>
/// Feature para gerar boleto de pagamento
/// </summary>
public class GenerateBoletoFeature(Guid paymentId) : Feature<FeatureResponse<GenerateBoletoResponse>>
{
    /// <summary>
    /// ID do pagamento
    /// </summary>
    public Guid PaymentId { get; set; } = paymentId;
}