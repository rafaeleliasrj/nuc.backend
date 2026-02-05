using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.ReviewCreate;

/// <summary>
/// Feature para criação de avaliação
/// </summary>
public class CreateReviewFeature : Feature<FeatureResponse<ReviewResponse>>
{
    /// <summary>
    /// Request de criação de avaliação
    /// </summary>
    public CreateReviewRequest Data { get; init; }
}