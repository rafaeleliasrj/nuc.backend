using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;
using NautiHub.Domain.ValueObjects;

namespace NautiHub.Application.UseCases.Features.PaymentCreate;

/// <summary>
/// Comando para criar pagamento no Asaas
/// </summary>
public class CreatePaymentFeature : Feature<FeatureResponse<CreatePaymentCommandResponse>>
{
    /// <summary>
    /// Request de criação de pagamento
    /// </summary>
    public CreatePaymentRequest Data { get; init; }
}
