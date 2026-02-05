using NautiHub.Core.Data;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;

namespace NautiHub.Domain.Repositories;

/// <summary>
/// Interface de repositório para passeios agendados.
/// </summary>
public interface IScheduledTourRepository : IRepository<ScheduledTour>
{
    /// <summary>
    /// Obtém passeios por ID da reserva.
    /// </summary>
    Task<IEnumerable<ScheduledTour>> GetByBookingIdAsync(Guid bookingId);
    
    /// <summary>
    /// Obtém passeios por ID do barco.
    /// </summary>
    Task<IEnumerable<ScheduledTour>> GetByBoatIdAsync(Guid boatId);
    
    /// <summary>
    /// Obtém passeios por ID do dono do barco.
    /// </summary>
    Task<IEnumerable<ScheduledTour>> GetByBoatOwnerIdAsync(Guid boatOwnerId);
    
    /// <summary>
    /// Obtém passeios por status.
    /// </summary>
    Task<IEnumerable<ScheduledTour>> GetByStatusAsync(ScheduledTourStatus status);
    
    /// <summary>
    /// Obtém passeios por data específica.
    /// </summary>
    Task<IEnumerable<ScheduledTour>> GetByDateAsync(DateOnly date);
    
    /// <summary>
    /// Obtém passeios em um intervalo de datas.
    /// </summary>
    Task<IEnumerable<ScheduledTour>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate);
    
    /// <summary>
    /// Obtém passeios disponíveis para reserva (data futura, status Scheduled).
    /// </summary>
    Task<IEnumerable<ScheduledTour>> GetAvailableToursAsync();
    
    /// <summary>
    /// Obtém passeios em conflito de horário para um barco.
    /// </summary>
    Task<IEnumerable<ScheduledTour>> GetConflictingToursAsync(Guid boatId, DateOnly date, TimeOnly startTime, TimeOnly endTime, Guid? excludeTourId = null);
    
    /// <summary>
    /// Obtém passeios de hoje para um dono de barco.
    /// </summary>
    Task<IEnumerable<ScheduledTour>> GetTodayToursForOwnerAsync(Guid boatOwnerId);
    
    /// <summary>
    /// Lista passeios com paginação e filtros.
    /// </summary>
    Task<(IEnumerable<ScheduledTour> items, int total)> ListAsync(
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
        string? orderBy = null);
}