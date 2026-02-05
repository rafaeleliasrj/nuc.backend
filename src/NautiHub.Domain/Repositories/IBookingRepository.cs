using NautiHub.Core.Data;
using NautiHub.Core.Messages.Models;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;

namespace NautiHub.Domain.Repositories;

/// <summary>
/// Interface do repositório de reservas
/// </summary>
public interface IBookingRepository : IRepository<Booking>
{
    /// <summary>
    /// Lista reservas paginada
    /// </summary>
    /// <param name="search"></param>
    /// <param name="dateCreatedStart"></param>
    /// <param name="dateCreatedEnd"></param>
    /// <param name="dateUpdatedStart"></param>
    /// <param name="dateUpdatedEnd"></param>
    /// <param name="page"></param>
    /// <param name="perPage"></param>
    /// <param name="orderBy"></param>
    /// <returns></returns>
    public Task<ListPaginationResponse<Booking>> ListAsync(
        string? search = null,
        DateTime? dateCreatedStart = null,
        DateTime? dateCreatedEnd = null,
        DateTime? dateUpdatedStart = null,
        DateTime? dateUpdatedEnd = null,
        int page = 1,
        int perPage = 10,
        string? orderBy = null
    );
    /// <summary>
    /// Buscar reserva por ID
    /// </summary>
    Task<Booking> GetByIdAsync(Guid id);

    /// <summary>
    /// Buscar reservas por usuário
    /// </summary>
    Task<List<Booking>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Buscar reservas por barco
    /// </summary>
    Task<List<Booking>> GetByBoatIdAsync(Guid boatId);

    /// <summary>
    /// Listar reservas por status
    /// </summary>
    Task<List<Booking>> GetByStatusAsync(BookingStatus status);
}