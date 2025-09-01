using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Data.Interfaces
{
    public interface IAddRangeRepository<TEntity>
        where TEntity : class
    {
        Task<int> ExecuteAsync(DbContext dbContext, IReadOnlyList<TEntity> entities);
    }
}
