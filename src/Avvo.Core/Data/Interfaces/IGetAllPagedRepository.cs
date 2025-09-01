using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Avvo.Core.Data.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Data.Interfaces
{
    public interface IGetAllPagedRepository<TEntity> where TEntity : class
    {
        Task<PagedResult<TEntity>> ExecuteAsync(DbContext dbContext, Expression<Func<TEntity, bool>>? predicate = null, IDictionary<Expression<Func<TEntity, object>>, string>? orderBy = null, int page = 1, int pageSize = 10);
    }
}
