namespace NautiHub.Application.UseCases.Models.Requests;

/// <summary>
/// Request para atualização de mensagem de chat
/// </summary>
public class UpdateChatMessageRequest
{
    /// <summary>
    /// Conteúdo atualizado da mensagem
    /// </summary>
    public string Message { get; set; }
}