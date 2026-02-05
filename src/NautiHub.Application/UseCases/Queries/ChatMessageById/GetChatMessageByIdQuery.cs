using NautiHub.Core.Messages.Queries;
using NautiHub.Application.UseCases.Models.Responses;

namespace NautiHub.Application.UseCases.Queries.ChatMessageById;

/// <summary>
/// Query para buscar mensagem de chat por ID
/// </summary>
public class GetChatMessageByIdQuery(Guid id) : Query<QueryResponse<ChatMessageResponse>>
{
    /// <summary>
    /// ID da mensagem de chat
    /// </summary>
    public Guid Id { get; set; } = id;
}