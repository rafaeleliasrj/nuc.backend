using Microsoft.EntityFrameworkCore;
using NautiHub.Core.Data;
using NautiHub.Core.Extensions;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Repositories;
using NautiHub.Infrastructure.DataContext;
using NautiHub.Domain.Enums;

namespace NautiHub.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de pagamentos.
/// </summary>
public class PaymentRepository(DatabaseContext context) 
    : Repository<Payment, DatabaseContext>(context), IPaymentRepository
{
    private IQueryable<Payment> MakeFilter(
        string? search,
        Guid? bookingId,
        PaymentStatus? status,
        PaymentMethod? paymentMethod,
        DateTime? dueDateStart,
        DateTime? dueDateEnd,
        DateTime? createdAtStart,
        DateTime? createdAtEnd,
        string? orderBy)
    {
        IQueryable<Payment> filter = _dbSet
            .Include(p => p.Booking)
            .Include(p => p.Splits);

        if (!string.IsNullOrEmpty(search))
        {
            filter = filter.Where(p =>
                p.Description.Contains(search) ||
                p.AsaasPaymentId.Contains(search) ||
                p.ExternalReference.Contains(search));
        }

        if (bookingId.HasValue)
            filter = filter.Where(p => p.BookingId == bookingId);

        if (status.HasValue)
            filter = filter.Where(p => p.Status == status);

        if (paymentMethod.HasValue)
            filter = filter.Where(p => p.Method == paymentMethod);

        if (dueDateStart.HasValue)
            filter = filter.Where(p => p.DueDate >= dueDateStart);

        if (dueDateEnd.HasValue)
            filter = filter.Where(p => p.DueDate <= dueDateEnd);

        if (createdAtStart.HasValue)
            filter = filter.Where(p => p.CreatedAt >= createdAtStart);

        if (createdAtEnd.HasValue)
            filter = filter.Where(p => p.CreatedAt <= createdAtEnd);

        if (!string.IsNullOrEmpty(orderBy))
            filter = filter.ApplyOrder(orderBy);

        return filter;
    }

    public async Task<IEnumerable<Payment>> GetByBookingIdAsync(Guid bookingId)
    {
        return await _dbSet
            .Include(p => p.Splits)
            .Where(p => p.BookingId == bookingId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Payment?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.Booking)
            .Include(p => p.Splits)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Payment?> GetByAsaasPaymentIdAsync(string asaasPaymentId)
    {
        if (string.IsNullOrWhiteSpace(asaasPaymentId))
            return null;

        return await _dbSet
            .Include(p => p.Booking)
            .Include(p => p.Splits)
            .FirstOrDefaultAsync(p => p.AsaasPaymentId == asaasPaymentId);
    }

    public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status)
    {
        return await _dbSet
            .Include(p => p.Booking)
            .Include(p => p.Splits)
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByPaymentMethodAsync(PaymentMethod paymentMethod)
    {
        return await _dbSet
            .Include(p => p.Booking)
            .Include(p => p.Splits)
            .Where(p => p.Method == paymentMethod)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(p => p.Booking)
            .Include(p => p.Splits)
            .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetOverduePaymentsAsync()
    {
        var today = DateTime.UtcNow.Date;
        
        return await _dbSet
            .Include(p => p.Booking)
            .Include(p => p.Splits)
            .Where(p => p.DueDate.HasValue && 
                       p.DueDate.Value < today && 
                       p.Status == PaymentStatus.Pending)
            .OrderBy(p => p.DueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPendingPaymentsAsync()
    {
        return await _dbSet
            .Include(p => p.Booking)
            .Include(p => p.Splits)
            .Where(p => p.Status == PaymentStatus.Pending)
            .OrderBy(p => p.DueDate)
            .ThenByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Payment> items, int total)> ListAsync(
        int page = 1,
        int perPage = 10,
        string? search = null,
        Guid? bookingId = null,
        PaymentStatus? status = null,
        PaymentMethod? paymentMethod = null,
        DateTime? dueDateStart = null,
        DateTime? dueDateEnd = null,
        DateTime? createdAtStart = null,
        DateTime? createdAtEnd = null,
        string? orderBy = null)
    {
        var filter = MakeFilter(search, bookingId, status, paymentMethod, dueDateStart, dueDateEnd, createdAtStart, createdAtEnd, orderBy);
        
        var result = await filter.GetPaginated(page, perPage);
        return (result.Data, result.RowCount);
    }
}