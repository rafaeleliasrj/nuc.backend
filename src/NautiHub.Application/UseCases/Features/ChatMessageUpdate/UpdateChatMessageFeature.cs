using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.ChatMessageUpdate;

/// <summary>
/// Feature para atualização de mensagem de chat
/// </summary>
public class UpdateChatMessageFeature : Feature<FeatureResponse<ChatMessageResponse>>
{
    /// <summary>
    /// Identificador da mensagem
    /// </summary>
    public Guid MessageId { get; init; }

    /// <summary>
    /// Request de atualização de mensagem de chat
    /// </summary>
    public UpdateChatMessageRequest Data { get; init; }
}