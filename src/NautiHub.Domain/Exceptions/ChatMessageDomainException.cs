using NautiHub.Core.DomainObjects;

namespace NautiHub.Domain.Exceptions;

/// <summary>
/// Exceções de domínio específicas para a entidade ChatMessage
/// </summary>
public class ChatMessageDomainException : DomainException
{
    public string MessageKey { get; }

    public ChatMessageDomainException(string messageKey, string defaultMessage) 
        : base(defaultMessage)
    {
        MessageKey = messageKey;
    }

    public ChatMessageDomainException(string messageKey, string defaultMessage, Exception innerException) 
        : base(defaultMessage, innerException)
    {
        MessageKey = messageKey;
    }

    // Métodos fáceis para criar exceções específicas
    public static ChatMessageDomainException NotFound() => 
        new("ChatMessage_Not_Found", "Mensagem de chat {0} não encontrada");

    public static ChatMessageDomainException BookingIdRequired() => 
        new("Validation_Booking_Id_Required", "Booking ID is required");

    public static ChatMessageDomainException SenderIdRequired() => 
        new("Validation_Customer_Id_Required", "Sender ID is required");

    public static ChatMessageDomainException MessageRequired() => 
        new("Validation_Comment_Too_Long", "Message is required");

    public static ChatMessageDomainException MessageTooLong() => 
        new("Validation_Comment_Too_Long", "Message cannot exceed 1000 characters");
}