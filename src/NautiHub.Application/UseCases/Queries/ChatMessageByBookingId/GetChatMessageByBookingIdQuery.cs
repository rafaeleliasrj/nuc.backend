using NautiHub.Core.Messages.Queries;
using NautiHub.Application.UseCases.Models.Responses;

namespace NautiHub.Application.UseCases.Queries.ChatMessageByBookingId;

/// <summary>
/// Query para buscar mensagens de chat por ID da reserva
/// </summary>
public class GetChatMessageByBookingIdQuery(Guid bookingId) : Query<QueryResponse<ChatMessageListResponse>>
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