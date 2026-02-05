using NautiHub.Core.DomainObjects;
using NautiHub.Domain.Exceptions;

namespace NautiHub.Domain.Entities;

/// <summary>
/// Entidade que representa uma mensagem de chat associada a uma reserva.
/// </summary>
public class ChatMessage : Entity, IAggregateRoot
{
    /// <summary>
    /// Identificador da reserva associada à mensagem.
    /// </summary>
    public Guid BookingId { get; private set; }

    /// <summary>
    /// Identificador do usuário remetente da mensagem.
    /// </summary>
    public Guid SenderId { get; private set; }

    /// <summary>
    /// Conteúdo da mensagem.
    /// </summary>
    public string Message { get; private set; } = string.Empty;

    /// <summary>
    /// Indica se a mensagem foi lida.
    /// </summary>
    public bool IsRead { get; private set; }

    // Construtor privado para EF Core
    private ChatMessage() { }

    /// <summary>
    /// Cria uma nova instância de ChatMessage.
    /// </summary>
    /// <param name="bookingId">Identificador da reserva.</param>
    /// <param name="senderId">Identificador do remetente.</param>
    /// <param name="message">Conteúdo da mensagem.</param>
    public ChatMessage(Guid bookingId, Guid senderId, string message)
    {
        ValidateParameters(bookingId, senderId, message);

        BookingId = bookingId;
        SenderId = senderId;
        Message = message;
        IsRead = false;
    }

    /// <summary>
    /// Valida os parâmetros de criação da mensagem.
    /// </summary>
    private static void ValidateParameters(Guid bookingId, Guid senderId, string message)
    {
        if (bookingId == Guid.Empty)
            throw ChatMessageDomainException.BookingIdRequired();

        if (senderId == Guid.Empty)
            throw ChatMessageDomainException.SenderIdRequired();

        if (string.IsNullOrWhiteSpace(message))
            throw ChatMessageDomainException.MessageRequired();

        if (message.Length > 1000)
            throw ChatMessageDomainException.MessageTooLong();
    }

    /// <summary>
    /// Marca a mensagem como lida.
    /// </summary>
    public void MarkAsRead()
    {
        IsRead = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marca a mensagem como não lida.
    /// </summary>
    public void MarkAsUnread()
    {
        IsRead = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza o conteúdo da mensagem.
    /// </summary>
    /// <param name="newMessage">Novo conteúdo da mensagem.</param>
    public void UpdateMessage(string newMessage)
    {
        if (string.IsNullOrWhiteSpace(newMessage))
            throw ChatMessageDomainException.MessageRequired();

        if (newMessage.Length > 1000)
            throw ChatMessageDomainException.MessageTooLong();

        Message = newMessage;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Verifica se a mensagem foi enviada por um usuário específico.
    /// </summary>
    /// <param name="userId">ID do usuário a verificar.</param>
    /// <returns>True se o usuário é o remetente da mensagem.</returns>
    public bool IsFromSender(Guid userId)
    {
        return SenderId == userId;
    }

    /// <summary>
    /// Verifica se a mensagem está associada a uma reserva específica.
    /// </summary>
    /// <param name="bookingId">ID da reserva a verificar.</param>
    /// <returns>True se a mensagem pertence à reserva informada.</returns>
    public bool BelongsToBooking(Guid bookingId)
    {
        return BookingId == bookingId;
    }
}