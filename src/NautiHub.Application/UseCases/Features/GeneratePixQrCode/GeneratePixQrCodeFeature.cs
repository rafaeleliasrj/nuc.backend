using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.GeneratePixQrCode;

/// <summary>
/// Feature para gerar QR Code Pix de pagamento
/// </summary>
public class GeneratePixQrCodeFeature(Guid paymentId) : Feature<FeatureResponse<GeneratePixQrCodeResponse>>
{
    /// <summary>
    /// ID do pagamento
    /// </summary>
    public Guid PaymentId { get; set; } = paymentId;
}