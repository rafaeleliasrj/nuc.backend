using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.ReviewDelete;

/// <summary>
/// Feature para exclusão de avaliação
/// </summary>
public class DeleteReviewFeature : Feature<FeatureResponse<bool>>
{
    /// <summary>
    /// Identificador da avaliação
    /// </summary>
    public Guid ReviewId { get; init; }
}