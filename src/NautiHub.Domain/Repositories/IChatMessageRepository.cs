using NautiHub.Core.Data;
using NautiHub.Domain.Entities;

namespace NautiHub.Domain.Repositories;

/// <summary>
/// Interface de repositório para mensagens de chat.
/// </summary>
public interface IChatMessageRepository : IRepository<ChatMessage>
{
    /// <summary>
    /// Obtém mensagens por ID da reserva.
    /// </summary>
    Task<IEnumerable<ChatMessage>> GetByBookingIdAsync(Guid bookingId);
    
    /// <summary>
    /// Obtém mensagens por ID do remetente.
    /// </summary>
    Task<IEnumerable<ChatMessage>> GetBySenderIdAsync(Guid senderId);
    
    /// <summary>
    /// Obtém mensagens por ID do destinatário.
    /// </summary>
    Task<IEnumerable<ChatMessage>> GetByRecipientIdAsync(Guid recipientId);
    
    /// <summary>
    /// Obtém mensagens não lidas para um usuário.
    /// </summary>
    Task<IEnumerable<ChatMessage>> GetUnreadMessagesAsync(Guid userId);
    
    /// <summary>
    /// Marca mensagens como lidas para um usuário.
    /// </summary>
    Task MarkMessagesAsReadAsync(Guid userId, IEnumerable<Guid> messageIds);
    
    /// <summary>
    /// Obtém conversa entre dois usuários para uma reserva.
    /// </summary>
    Task<IEnumerable<ChatMessage>> GetConversationAsync(Guid user1Id, Guid user2Id, Guid bookingId);
    
    /// <summary>
    /// Lista mensagens com paginação e filtros.
    /// </summary>
    Task<(IEnumerable<ChatMessage> items, int total)> ListAsync(
        int page = 1,
        int perPage = 10,
        string? search = null,
        Guid? bookingId = null,
        Guid? senderId = null,
        Guid? recipientId = null,
        DateTime? createdAtStart = null,
        DateTime? createdAtEnd = null,
        string? orderBy = null);
}