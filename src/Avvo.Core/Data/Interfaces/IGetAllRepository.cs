using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Data.Interfaces
{
    public interface IGetAllRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> ExecuteAsync(DbContext dbContext, Expression<Func<TEntity, bool>>? predicate = null, int limit = 10, IDictionary<Expression<Func<TEntity, object>>, string>? orderBy = null, CancellationToken cancellationToken = default);
    }
}
