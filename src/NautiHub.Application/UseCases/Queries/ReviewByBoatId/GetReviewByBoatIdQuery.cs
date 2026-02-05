using NautiHub.Core.Messages.Queries;
using NautiHub.Application.UseCases.Models.Responses;

namespace NautiHub.Application.UseCases.Queries.ReviewByBoatId;

/// <summary>
/// Query para buscar avaliações por ID do barco
/// </summary>
public class GetReviewByBoatIdQuery(Guid boatId) : Query<QueryResponse<ReviewListResponse>>
{
    /// <summary>
    /// ID do barco
    /// </summary>
    public Guid BoatId { get; set; } = boatId;

    /// <summary>
    /// Página atual
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Quantidade de itens por página
    /// </summary>
    public int PageSize { get; set; } = 50;
}