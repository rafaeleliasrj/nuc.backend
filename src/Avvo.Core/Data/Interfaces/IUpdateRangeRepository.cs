using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Data.Interfaces
{
    public interface IUpdateRangeRepository<TEntity> where TEntity : class
    {
        Task<int> ExecuteAsync(DbContext dbContext, IEnumerable<TEntity> entities);
    }
}
