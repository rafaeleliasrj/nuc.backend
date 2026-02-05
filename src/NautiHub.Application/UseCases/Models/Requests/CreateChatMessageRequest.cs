namespace NautiHub.Application.UseCases.Models.Requests;

/// <summary>
/// Request para criação de mensagem de chat
/// </summary>
public class CreateChatMessageRequest
{
    /// <summary>
    /// Identificador da reserva associada à mensagem
    /// </summary>
    public Guid BookingId { get; set; }

    /// <summary>
    /// Identificador do usuário remetente da mensagem
    /// </summary>
    public Guid SenderId { get; set; }

    /// <summary>
    /// Conteúdo da mensagem
    /// </summary>
    public string Message { get; set; }
}