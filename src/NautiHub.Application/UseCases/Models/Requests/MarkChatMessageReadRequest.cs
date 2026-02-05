namespace NautiHub.Application.UseCases.Models.Requests;

/// <summary>
/// Request para marcar mensagem como lida/não lida
/// </summary>
public class MarkChatMessageReadRequest
{
    /// <summary>
    /// Indica se a mensagem deve ser marcada como lida
    /// </summary>
    public bool IsRead { get; set; }
}