using NautiHub.Core.Messages.Queries;
using NautiHub.Application.UseCases.Models.Responses;

namespace NautiHub.Application.UseCases.Queries.ReviewByBookingId;

/// <summary>
/// Query para buscar avaliações por ID da reserva
/// </summary>
public class GetReviewByBookingIdQuery(Guid bookingId) : Query<QueryResponse<ReviewListResponse>>
{
    /// <summary>
    /// ID da reserva
    /// </summary>
    public Guid BookingId { get; set; } = bookingId;

    /// <summary>
    /// Página atual
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Quantidade de itens por página
    /// </summary>
    public int PageSize { get; set; } = 50;
}