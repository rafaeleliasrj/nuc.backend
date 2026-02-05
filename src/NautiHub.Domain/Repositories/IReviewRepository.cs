using NautiHub.Core.Data;
using NautiHub.Domain.Entities;

namespace NautiHub.Domain.Repositories;

/// <summary>
/// Interface de repositório para avaliações.
/// </summary>
public interface IReviewRepository : IRepository<Review>
{
    /// <summary>
    /// Obtém avaliações por ID da reserva.
    /// </summary>
    Task<IEnumerable<Review>> GetByBookingIdAsync(Guid bookingId);
    
    /// <summary>
    /// Obtém avaliações por ID do barco.
    /// </summary>
    Task<IEnumerable<Review>> GetByBoatIdAsync(Guid boatId);
    
    /// <summary>
    /// Obtém avaliações por ID do avaliador.
    /// </summary>
    Task<IEnumerable<Review>> GetByReviewerIdAsync(Guid reviewerId);
    
    /// <summary>
    /// Obtém avaliações por ID do avaliado (dono do barco).
    /// </summary>
    Task<IEnumerable<Review>> GetByRevieweeIdAsync(Guid revieweeId);
    
    /// <summary>
    /// Obtém avaliações por nota.
    /// </summary>
    Task<IEnumerable<Review>> GetByRatingAsync(int rating);
    
    /// <summary>
    /// Verifica se usuário já avaliou uma reserva.
    /// </summary>
    Task<bool> HasUserReviewedBookingAsync(Guid reviewerId, Guid bookingId);
    
    /// <summary>
    /// Calcula média de avaliações de um barco.
    /// </summary>
    Task<double> GetAverageRatingForBoatAsync(Guid boatId);
    
    /// <summary>
    /// Obtém avaliações com paginação e filtros.
    /// </summary>
    Task<(IEnumerable<Review> items, int total)> ListAsync(
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
        string? orderBy = null);
}