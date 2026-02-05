using NautiHub.Core.Messages.Queries;
using NautiHub.Application.UseCases.Models.Responses;

namespace NautiHub.Application.UseCases.Queries.ChatMessageList;

/// <summary>
/// Query para listar mensagens de chat
/// </summary>
public class GetChatMessageListQuery : Query<QueryResponse<ChatMessageListResponse>>
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
    /// Termo de busca
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// ID da reserva filtrada
    /// </summary>
    public Guid? BookingId { get; set; }

    /// <summary>
    /// ID do remetente filtrado
    /// </summary>
    public Guid? SenderId { get; set; }

    /// <summary>
    /// Data inicial do período
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// Data final do período
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// Campo de ordenação
    /// </summary>
    public string? OrderBy { get; set; }
}