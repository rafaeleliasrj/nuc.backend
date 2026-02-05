using Microsoft.EntityFrameworkCore;
using NautiHub.Core.Data;
using NautiHub.Core.Extensions;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;
using NautiHub.Domain.Repositories;
using NautiHub.Infrastructure.DataContext;

namespace NautiHub.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de passeios agendados.
/// </summary>
public class ScheduledTourRepository(DatabaseContext context) 
    : Repository<ScheduledTour, DatabaseContext>(context), IScheduledTourRepository
{
    private IQueryable<ScheduledTour> MakeFilter(
        string? search,
        Guid? boatId,
        Guid? boatOwnerId,
        ScheduledTourStatus? status,
        DateOnly? date,
        DateOnly? dateStart,
        DateOnly? dateEnd,
        decimal? minPrice,
        decimal? maxPrice,
        string? orderBy)
    {
        IQueryable<ScheduledTour> filter = _dbSet
            .Include(st => st.Boat);

        if (!string.IsNullOrEmpty(search))
        {
            filter = filter.Where(st =>
                (st.Notes != null && st.Notes.Contains(search)) ||
                (st.Boat.Name != null && st.Boat.Name.Contains(search)));
        }

        if (boatId.HasValue)
            filter = filter.Where(st => st.BoatId == boatId);

        // Removido filtro por boatOwnerId pois não existe propriedade UserId
        if (status.HasValue)
            filter = filter.Where(st => st.Status == status);

        if (date.HasValue)
            filter = filter.Where(st => st.TourDate == date);

        if (dateStart.HasValue)
            filter = filter.Where(st => st.TourDate >= dateStart);

        if (dateEnd.HasValue)
            filter = filter.Where(st => st.TourDate <= dateEnd);

        // Removidos filtros de preço pois não existem na entidade ScheduledTour

        if (!string.IsNullOrEmpty(orderBy))
            filter = filter.ApplyOrder(orderBy);

        return filter;
    }

    public async Task<IEnumerable<ScheduledTour>> GetByBookingIdAsync(Guid bookingId)
    {
        // Como não existe BookingId, retornamos lista vazia ou poderíamos fazer join se necessário
        return await Task.FromResult<IEnumerable<ScheduledTour>>(new List<ScheduledTour>());
    }

    public async Task<IEnumerable<ScheduledTour>> GetByBoatIdAsync(Guid boatId)
    {
        return await _dbSet
            .Include(st => st.Boat)
            .Where(st => st.BoatId == boatId)
            .OrderBy(st => st.TourDate)
            .ThenBy(st => st.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<ScheduledTour>> GetByBoatOwnerIdAsync(Guid boatOwnerId)
    {
        // Como não existe UserId, filtramos pelo Boat.UserId
        return await _dbSet
            .Include(st => st.Boat)
            .Where(st => st.Boat.UserId == boatOwnerId)
            .OrderBy(st => st.TourDate)
            .ThenBy(st => st.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<ScheduledTour>> GetByStatusAsync(ScheduledTourStatus status)
    {
        return await _dbSet
            .Include(st => st.Boat)
            .Where(st => st.Status == status)
            .OrderBy(st => st.TourDate)
            .ThenBy(st => st.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<ScheduledTour>> GetByDateAsync(DateOnly date)
    {
        return await _dbSet
            .Include(st => st.Boat)
            .Where(st => st.TourDate == date)
            .OrderBy(st => st.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<ScheduledTour>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate)
    {
        return await _dbSet
            .Include(st => st.Boat)
            .Where(st => st.TourDate >= startDate && st.TourDate <= endDate)
            .OrderBy(st => st.TourDate)
            .ThenBy(st => st.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<ScheduledTour>> GetAvailableToursAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        return await _dbSet
            .Include(st => st.Boat)
            .Where(st => st.TourDate >= today && 
                        st.Status == ScheduledTourStatus.Scheduled &&
                        st.AvailableSeats > 0)
            .OrderBy(st => st.TourDate)
            .ThenBy(st => st.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<ScheduledTour>> GetConflictingToursAsync(Guid boatId, DateOnly date, TimeOnly startTime, TimeOnly endTime, Guid? excludeTourId = null)
    {
        var query = _dbSet
            .Include(st => st.Boat)
            .Where(st => st.BoatId == boatId && 
                        st.TourDate == date &&
                        st.Status == ScheduledTourStatus.Scheduled &&
                        ((st.StartTime < endTime && st.EndTime > startTime))); // Overlap logic

        if (excludeTourId.HasValue)
            query = query.Where(st => st.Id != excludeTourId);

        return await query
            .OrderBy(st => st.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<ScheduledTour>> GetTodayToursForOwnerAsync(Guid boatOwnerId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        return await _dbSet
            .Include(st => st.Boat)
            .Where(st => st.Boat.UserId == boatOwnerId && st.TourDate == today)
            .OrderBy(st => st.StartTime)
            .ToListAsync();
    }

    public async Task<(IEnumerable<ScheduledTour> items, int total)> ListAsync(
        int page = 1,
        int perPage = 10,
        string? search = null,
        Guid? boatId = null,
        Guid? boatOwnerId = null,
        ScheduledTourStatus? status = null,
        DateOnly? date = null,
        DateOnly? dateStart = null,
        DateOnly? dateEnd = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? orderBy = null)
    {
        var filter = MakeFilter(search, boatId, boatOwnerId, status, date, dateStart, dateEnd, minPrice, maxPrice, orderBy);
        
        var result = await filter.GetPaginated(page, perPage);
        return (result.Data, result.RowCount);
    }
}