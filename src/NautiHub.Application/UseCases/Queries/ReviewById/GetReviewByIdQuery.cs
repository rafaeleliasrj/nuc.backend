using NautiHub.Core.Messages.Queries;
using NautiHub.Application.UseCases.Models.Responses;

namespace NautiHub.Application.UseCases.Queries.ReviewById;

/// <summary>
/// Query para buscar avaliação por ID
/// </summary>
public class GetReviewByIdQuery(Guid id) : Query<QueryResponse<ReviewResponse>>
{
    /// <summary>
    /// ID da avaliação
    /// </summary>
    public Guid Id { get; set; } = id;
}