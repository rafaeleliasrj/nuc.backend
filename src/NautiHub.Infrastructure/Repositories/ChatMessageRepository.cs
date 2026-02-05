using Microsoft.EntityFrameworkCore;
using NautiHub.Core.Data;
using NautiHub.Core.Extensions;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Repositories;
using NautiHub.Infrastructure.DataContext;

namespace NautiHub.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de mensagens de chat.
/// </summary>
public class ChatMessageRepository(DatabaseContext context) 
    : Repository<ChatMessage, DatabaseContext>(context), IChatMessageRepository
{
    private IQueryable<ChatMessage> MakeFilter(
        string? search,
        Guid? bookingId,
        Guid? senderId,
        DateTime? createdAtStart,
        DateTime? createdAtEnd,
        string? orderBy)
    {
        IQueryable<ChatMessage> filter = _dbSet;

        if (!string.IsNullOrEmpty(search))
        {
            filter = filter.Where(m =>
                m.Message.Contains(search));
        }

        if (bookingId.HasValue)
            filter = filter.Where(m => m.BookingId == bookingId);

        if (senderId.HasValue)
            filter = filter.Where(m => m.SenderId == senderId);

        if (createdAtStart.HasValue)
            filter = filter.Where(m => m.CreatedAt >= createdAtStart);

        if (createdAtEnd.HasValue)
            filter = filter.Where(m => m.CreatedAt <= createdAtEnd);

        if (!string.IsNullOrEmpty(orderBy))
            filter = filter.ApplyOrder(orderBy);

        return filter;
    }

    public async Task<IEnumerable<ChatMessage>> GetByBookingIdAsync(Guid bookingId)
    {
        return await _dbSet
            .Where(m => m.BookingId == bookingId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ChatMessage>> GetBySenderIdAsync(Guid senderId)
    {
        return await _dbSet
            .Where(m => m.SenderId == senderId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ChatMessage>> GetByRecipientIdAsync(Guid recipientId)
    {
        return await _dbSet
            .Where(m => m.BookingId != Guid.Empty) // Mensagens sem destinatário específico são globais à reserva
            .ToListAsync();
    }

    public async Task<IEnumerable<ChatMessage>> GetUnreadMessagesAsync(Guid userId)
    {
        return await _dbSet
            .Where(m => m.SenderId != userId && !m.IsRead) // Mensagens de outros usuários não lidas
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task MarkMessagesAsReadAsync(Guid userId, IEnumerable<Guid> messageIds)
    {
        var messages = await _dbSet
            .Where(m => messageIds.Contains(m.Id))
            .ToListAsync();

        foreach (var message in messages)
        {
            message.MarkAsRead();
        }

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ChatMessage>> GetConversationAsync(Guid user1Id, Guid user2Id, Guid bookingId)
    {
        return await _dbSet
            .Where(m => m.BookingId == bookingId &&
                        (m.SenderId == user1Id || m.SenderId == user2Id))
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<(IEnumerable<ChatMessage> items, int total)> ListAsync(
        int page = 1,
        int perPage = 10,
        string? search = null,
        Guid? bookingId = null,
        Guid? senderId = null,
        Guid? recipientId = null,
        DateTime? createdAtStart = null,
        DateTime? createdAtEnd = null,
        string? orderBy = null)
    {
        var filter = MakeFilter(search, bookingId, senderId, createdAtStart, createdAtEnd, orderBy);
        
        var result = await filter.GetPaginated(page, perPage);
        return (result.Data, result.RowCount);
    }
}