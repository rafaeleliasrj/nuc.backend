using NautiHub.Core.Data;
using NautiHub.Core.Messages.Models;
using NautiHub.Domain.Entities;
using System.Linq.Expressions;

namespace NautiHub.Domain.Repositories;

public interface IBoatRepository : IRepository<Boat>
{
    public Task<Boat?> GetByIdAsync(Guid id, bool ignoreQueryFilter = false);
    public Task<ListPaginationResponse<Boat>> ListAsync(
        string? search = null,
        DateTime? dateCreatedStart = null,
        DateTime? dateCreatedEnd = null,
        DateTime? dateUpdatedStart = null,
        DateTime? dateUpdatedEnd = null,
        int page = 1,
        int perPage = 10,
        string? orderBy = null
    );

    public Task<bool> ExistAsync(Expression<Func<Boat, bool>>? predicate = null);

    public Task<Boat?> GetByPredicateAsync(Expression<Func<Boat, bool>>? predicate = null, bool ignoreQueryFilter = false);
}
