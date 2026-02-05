using Microsoft.EntityFrameworkCore;
using NautiHub.Core.Data;
using NautiHub.Core.Extensions;
using NautiHub.Core.Messages.Models;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;
using NautiHub.Domain.Repositories;
using NautiHub.Infrastructure.DataContext;

namespace NautiHub.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de reservas
/// </summary>
public class BookingRepository(DatabaseContext context)
        : Repository<Booking, DatabaseContext>(context), IBookingRepository
{
    private IQueryable<Booking> MakeFilter(
        string? search,
        DateTime? dateCreatedStart,
        DateTime? dateCreatedEnd,
        DateTime? dateUpdatedStart,
        DateTime? dateUpdatedEnd,
        string? orderBy
    )
    {
        IQueryable<Booking> filter = _dbSet;

        if (!string.IsNullOrEmpty(search))
        {
            filter = filter.Where(w =>
                (w.BookingNumber != null && w.BookingNumber.Contains(search))
            );
        }

        if (dateCreatedStart != null)
            filter = filter.Where(w => w.CreatedAt >= dateCreatedStart);

        if (dateCreatedEnd != null)
            filter = filter.Where(w => w.CreatedAt <= dateCreatedEnd);

        if (dateUpdatedStart != null)
            filter = filter.Where(w => w.UpdatedAt >= dateUpdatedStart);

        if (dateUpdatedEnd != null)
            filter = filter.Where(w => w.UpdatedAt <= dateUpdatedEnd);

        if (!string.IsNullOrEmpty(orderBy))
            filter = filter.ApplyOrder(orderBy);

        return filter;
    }

    public async Task<ListPaginationResponse<Booking>> ListAsync(
        string? search = null,
        DateTime? dateCreatedStart = null,
        DateTime? dateCreatedEnd = null,
        DateTime? dateUpdatedStart = null,
        DateTime? dateUpdatedEnd = null,
        int page = 1,
        int perPage = 10,
        string? orderBy = null
    )
    {
        IQueryable<Booking> filter = MakeFilter(
            search,
            dateCreatedStart,
            dateCreatedEnd,
            dateUpdatedStart,
            dateUpdatedEnd,
            orderBy
        );

        ListPaginationResponse<Booking> list = await filter.GetPaginated(page, perPage);
        return list;
    }

    public async Task<Booking> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(b => b.Boat)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<List<Booking>> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Booking>> GetByBoatIdAsync(Guid boatId)
    {
        return await _dbSet
            .Where(b => b.BoatId == boatId)
            .ToListAsync();
    }

    public async Task<List<Booking>> GetByStatusAsync(BookingStatus status)
    {
        return await _dbSet
            .Where(b => b.Status == status)
            .ToListAsync();
    }
}