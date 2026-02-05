using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.ChatMessageCreate;

/// <summary>
/// Feature para criação de mensagem de chat
/// </summary>
public class CreateChatMessageFeature : Feature<FeatureResponse<ChatMessageResponse>>
{
    /// <summary>
    /// Request de criação de mensagem de chat
    /// </summary>
    public CreateChatMessageRequest Data { get; init; }
}