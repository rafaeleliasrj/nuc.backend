namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response de lista de mensagens de chat
/// </summary>
public class ChatMessageListResponse
{
    /// <summary>
    /// Lista de mensagens
    /// </summary>
    public IList<ChatMessageResponse> Messages { get; set; } = new List<ChatMessageResponse>();

    /// <summary>
    /// Total de mensagens encontradas
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// Página atual
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Quantidade de itens por página
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total de páginas
    /// </summary>
    public int TotalPages { get; set; }
}