using Microsoft.EntityFrameworkCore;
using NautiHub.Core.Data;
using NautiHub.Core.Extensions;
using NautiHub.Core.Messages.Models;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Repositories;
using NautiHub.Infrastructure.DataContext;
using System.Linq.Expressions;

namespace NautiHub.Infrastructure.Repositories;

/// <summary>
/// Implementação completa do repositório de Barcos estendendo o EmpresaRepository
/// </summary>
public class BoatRepository(DatabaseContext context)
        : Repository<Boat, DatabaseContext>(context), IBoatRepository
{
    private IQueryable<Boat> MakeFilter(
        string? search,
        DateTime? dateCreatedStart,
        DateTime? dateCreatedEnd,
        DateTime? dateUpdatedStart,
        DateTime? dateUpdatedEnd,
        string? orderBy
    )
    {
        IQueryable<Boat> filter = _dbSet;

        if (!string.IsNullOrEmpty(search))
        {
            filter = filter.Where(w =>
                (w.Name != null && w.Name.Contains(search))
                || (w.Name != null && w.Name.Contains(search))
                || (w.Description != null && w.Description.Contains(search))
                || (w.LocationCity != null && w.LocationCity.Contains(search))
                || (w.LocationCity != null && w.LocationCity.Contains(search))
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

    public async Task<ListPaginationResponse<Boat>> ListAsync(
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
        IQueryable<Boat> filter = MakeFilter(
            search,
            dateCreatedStart,
            dateCreatedEnd,
            dateUpdatedStart,
            dateUpdatedEnd,
            orderBy
        );

        ListPaginationResponse<Boat> list = await filter.GetPaginated(page, perPage);
        return list;
    }

    public async Task<Boat?> GetByIdAsync(Guid id, bool ignoreQueryFilter = false)
    {
        IQueryable<Boat> query = _dbSet
            .Include(b => b.Images);

        if (ignoreQueryFilter)
            query = query.IgnoreQueryFilters();

        return await query.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<bool> ExistAsync(Expression<Func<Boat, bool>>? predicate = null)
    {
        if (predicate != null)
            return await _dbSet.AnyAsync(predicate);

        return await _dbSet.AnyAsync();
    }

    public async Task<Boat?> GetByPredicateAsync(Expression<Func<Boat, bool>>? predicate = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _dbSet.IgnoreQueryFilters() : _dbSet;
        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<System.Collections.Generic.List<Boat>> GetByLocationAsync(string city, string state)
    {
        return await _dbSet
            .Include(b => b.Images)
            .Where(b => b.LocationCity == city && b.LocationState == state && b.IsActive)
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<System.Collections.Generic.List<Boat>> GetActiveBoatsAsync()
    {
        return await _dbSet
            .Include(b => b.Images)
            .Where(b => b.IsActive && b.Status == Domain.Enums.BoatStatus.Approved)
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<System.Collections.Generic.List<Boat>> GetByTypeAsync(Domain.Enums.BoatType boatType)
    {
        return await _dbSet
            .Include(b => b.Images)
            .Where(b => b.BoatType == boatType && b.IsActive)
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<System.Collections.Generic.List<Boat>> GetPendingApprovalAsync()
    {
        return await _dbSet
            .Where(b => b.Status == Domain.Enums.BoatStatus.Pending)
            .OrderBy(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<Boat> AddAsync(Boat boat)
    {
        await _dbSet.AddAsync(boat);
        await _context.SaveChangesAsync();
        return boat;
    }

    public async Task<Boat> UpdateAsync(Boat boat)
    {
        _dbSet.Update(boat);
        await _context.SaveChangesAsync();
        return boat;
    }

    public async Task DeleteAsync(Boat boat)
    {
        _dbSet.Remove(boat);
        await _context.SaveChangesAsync();
    }
}