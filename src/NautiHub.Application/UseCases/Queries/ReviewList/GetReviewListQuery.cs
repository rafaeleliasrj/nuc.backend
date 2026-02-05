using NautiHub.Core.Messages.Queries;
using NautiHub.Application.UseCases.Models.Responses;

namespace NautiHub.Application.UseCases.Queries.ReviewList;

/// <summary>
/// Query para listar avaliações
/// </summary>
public class GetReviewListQuery : Query<QueryResponse<ReviewListResponse>>
{
    /// <summary>
    /// Página atual
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Quantidade de itens por página
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// ID da reserva filtrada
    /// </summary>
    public Guid? BookingId { get; set; }

    /// <summary>
    /// ID do barco filtrado
    /// </summary>
    public Guid? BoatId { get; set; }

    /// <summary>
    /// ID do cliente filtrado
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Classificação mínima
    /// </summary>
    public int? MinRating { get; set; }

    /// <summary>
    /// Classificação máxima
    /// </summary>
    public int? MaxRating { get; set; }

    /// <summary>
    /// Data inicial do período
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Data final do período
    /// </summary>
    public DateTime? EndDate { get; set; }
}