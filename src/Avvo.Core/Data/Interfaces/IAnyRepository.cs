using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Data.Interfaces
{
    public interface IAnyRepository<TEntity> where TEntity : class
    {
        Task<bool> ExecuteAsync(DbContext dbContext, Expression<Func<TEntity, bool>> predicate);
    }
}
