using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;

namespace NautiHub.Application.UseCases.Features.ChatMessageDelete;

/// <summary>
/// Feature para exclusão de mensagem de chat
/// </summary>
public class DeleteChatMessageFeature : Feature<FeatureResponse<bool>>
{
    /// <summary>
    /// Identificador da mensagem
    /// </summary>
    public Guid MessageId { get; init; }
}