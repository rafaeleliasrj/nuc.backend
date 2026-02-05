namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response de mensagem de chat
/// </summary>
public class ChatMessageResponse
{
    /// <summary>
    /// Identificador da mensagem
    /// </summary>
    public Guid Id { get; set; }

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

    /// <summary>
    /// Indica se a mensagem foi lida
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Data de criação da mensagem
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data da última atualização
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}