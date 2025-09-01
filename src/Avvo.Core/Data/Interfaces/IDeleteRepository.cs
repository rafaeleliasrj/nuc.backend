using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Data.Interfaces
{
    public interface IDeleteRepository<TEntity> where TEntity : class
    {
        Task<int> ExecuteAsync(DbContext dbContext, object id, bool propagateEvent = false, IList<int>? propagateDestination = null, string? overrideEntityName = null);
    }
}
