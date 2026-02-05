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
/// Implementação do repositório de usuários
/// </summary>
public class UserRepository(DatabaseContext context)
        : Repository<User, DatabaseContext>(context), IUserRepository
{
    private IQueryable<User> MakeFilter(
        string? search,
        DateTime? dateCreatedStart,
        DateTime? dateCreatedEnd,
        DateTime? dateUpdatedStart,
        DateTime? dateUpdatedEnd,
        string? orderBy
    )
    {
        IQueryable<User> filter = _dbSet;

        if (!string.IsNullOrEmpty(search))
        {
            filter = filter.Where(w =>
                (w.FullName != null && w.FullName.Contains(search))
                || (w.UserName != null && w.UserName.Contains(search))
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

    public async Task<ListPaginationResponse<User>> ListAsync(
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
        IQueryable<User> filter = MakeFilter(
            search,
            dateCreatedStart,
            dateCreatedEnd,
            dateUpdatedStart,
            dateUpdatedEnd,
            orderBy
        );

        ListPaginationResponse<User> list = await filter.GetPaginated(page, perPage);
        return list;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return null;

        return await _dbSet
            .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return await _dbSet
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<bool> UserNameExistsAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return false;

        return await _dbSet
            .AnyAsync(u => u.UserName.ToLower() == userName.ToLower());
    }

    public async Task<System.Collections.Generic.List<User>> GetByUserTypeAsync(UserType userType)
    {
        return await _dbSet
            .Where(u => u.UserType == userType)
            .OrderBy(u => u.FullName)
            .ToListAsync();
    }

    public async Task<System.Collections.Generic.List<User>> GetHostsPendingApprovalAsync()
    {
        return await _dbSet
            .Where(u => u.UserType == UserType.Host || u.UserType == UserType.Both)
            .ToListAsync();
    }

    public async Task<System.Collections.Generic.List<User>> GetActiveHostsAsync()
    {
        return await _dbSet
            .Where(u => u.UserType == UserType.Host || u.UserType == UserType.Both)
            .ToListAsync();
    }
}