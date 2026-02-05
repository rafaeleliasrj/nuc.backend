using Microsoft.EntityFrameworkCore;
using NautiHub.Core.Data;
using NautiHub.Core.Extensions;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Repositories;
using NautiHub.Infrastructure.DataContext;

namespace NautiHub.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de avaliações.
/// </summary>
public class ReviewRepository(DatabaseContext context) 
    : Repository<Review, DatabaseContext>(context), IReviewRepository
{
    private IQueryable<Review> MakeFilter(
        string? search,
        Guid? boatId,
        Guid? customerId,
        int? minRating,
        int? maxRating,
        DateTime? createdAtStart,
        DateTime? createdAtEnd,
        string? orderBy)
    {
        IQueryable<Review> filter = _dbSet
            .Include(r => r.Booking)
            .Include(r => r.Boat)
            .Include(r => r.Customer);

        if (!string.IsNullOrEmpty(search))
        {
            filter = filter.Where(r =>
                (r.Comment != null && r.Comment.Contains(search)));
        }

        if (boatId.HasValue)
            filter = filter.Where(r => r.BoatId == boatId);

        if (customerId.HasValue)
            filter = filter.Where(r => r.CustomerId == customerId);

        if (minRating.HasValue)
            filter = filter.Where(r => r.Rating >= minRating);

        if (maxRating.HasValue)
            filter = filter.Where(r => r.Rating <= maxRating);

        if (createdAtStart.HasValue)
            filter = filter.Where(r => r.CreatedAt >= createdAtStart);

        if (createdAtEnd.HasValue)
            filter = filter.Where(r => r.CreatedAt <= createdAtEnd);

        if (!string.IsNullOrEmpty(orderBy))
            filter = filter.ApplyOrder(orderBy);

        return filter;
    }

    public async Task<IEnumerable<Review>> GetByBookingIdAsync(Guid bookingId)
    {
        return await _dbSet
            .Include(r => r.Customer)
            .Include(r => r.Boat)
            .Where(r => r.BookingId == bookingId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByBoatIdAsync(Guid boatId)
    {
        return await _dbSet
            .Include(r => r.Booking)
            .Include(r => r.Customer)
            .Where(r => r.BoatId == boatId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByReviewerIdAsync(Guid reviewerId)
    {
        return await _dbSet
            .Include(r => r.Booking)
            .Include(r => r.Boat)
            .Where(r => r.CustomerId == reviewerId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByRevieweeIdAsync(Guid revieweeId)
    {
        // Como não temos RevieweeId, vamos buscar por barcos deste usuário
        return await _dbSet
            .Include(r => r.Booking)
            .Include(r => r.Customer)
            .Include(r => r.Boat)
            .Where(r => r.Boat.UserId == revieweeId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByRatingAsync(int rating)
    {
        return await _dbSet
            .Include(r => r.Booking)
            .Include(r => r.Customer)
            .Include(r => r.Boat)
            .Where(r => r.Rating == rating)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> HasUserReviewedBookingAsync(Guid reviewerId, Guid bookingId)
    {
        return await _dbSet
            .AnyAsync(r => r.CustomerId == reviewerId && r.BookingId == bookingId);
    }

    public async Task<double> GetAverageRatingForBoatAsync(Guid boatId)
    {
        var reviews = await _dbSet
            .Where(r => r.BoatId == boatId)
            .ToListAsync();

        if (!reviews.Any())
            return 0.0;

        return reviews.Average(r => r.Rating);
    }

    public async Task<(IEnumerable<Review> items, int total)> ListAsync(
        int page = 1,
        int perPage = 10,
        string? search = null,
        Guid? boatId = null,
        Guid? reviewerId = null,
        Guid? revieweeId = null,
        int? minRating = null,
        int? maxRating = null,
        DateTime? createdAtStart = null,
        DateTime? createdAtEnd = null,
        string? orderBy = null)
    {
        // Para revieweeId, filtramos por barcos do usuário
        var filter = revieweeId.HasValue
            ? _dbSet.Include(r => r.Booking).Include(r => r.Boat).Include(r => r.Customer)
                .Where(r => r.Boat.UserId == revieweeId)
            : MakeFilter(search, boatId, reviewerId, minRating, maxRating, createdAtStart, createdAtEnd, orderBy);

        // Aplicar filtros adicionais se não for revieweeId
        if (!revieweeId.HasValue)
        {
            filter = MakeFilter(search, boatId, reviewerId, minRating, maxRating, createdAtStart, createdAtEnd, orderBy);
        }

        var result = await filter.GetPaginated(page, perPage);
        return (result.Data, result.RowCount);
    }
}