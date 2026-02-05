using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.ReviewUpdate;

/// <summary>
/// Feature para atualização de avaliação
/// </summary>
public class UpdateReviewFeature : Feature<FeatureResponse<ReviewResponse>>
{
    /// <summary>
    /// Identificador da avaliação
    /// </summary>
    public Guid ReviewId { get; init; }

    /// <summary>
    /// Request de atualização de avaliação
    /// </summary>
    public UpdateReviewRequest Data { get; init; }
}