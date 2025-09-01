using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Data.Interfaces
{
    public interface ICountRepository<TEntity> where TEntity : class
    {
        Task<long> ExecuteAsync(DbContext dbContext, Expression<Func<TEntity, bool>> predicate = null);
    }
}
