using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Data.Pagination
{
    public static class PagedListExtensions
    {
        public static async Task<PagedResult<TEntity>> GetPagedAsync<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, object>> predicateOrder, string orderType, int page, int pageSize) where TEntity : class
        {
            var result = new PagedResult<TEntity>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;

            result.Results =
                orderType.ToUpper() == "DESC" ?
                    await query.OrderByDescending(predicateOrder).Skip(skip).Take(pageSize).ToListAsync() :
                    await query.OrderBy(predicateOrder).Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }

        public static async Task<PagedResult<TEntity>> GetPagedAsync<TEntity>(this IQueryable<TEntity> query, IDictionary<Expression<Func<TEntity, object>>, string>? orderBy, int page, int pageSize) where TEntity : class
        {
            var result = new PagedResult<TEntity>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;

            bool isFirstOrder = true;

            if (orderBy != null)
                foreach (var item in orderBy)
                    if (isFirstOrder)
                    {
                        query = item.Value.ToUpper() == "DESC" ? query.OrderByDescending(item.Key) : query.OrderBy(item.Key);
                        isFirstOrder = false;
                    }
                    else
                    {
                        query = item.Value.ToUpper() == "DESC" ? ((IOrderedQueryable<TEntity>)query).ThenByDescending(item.Key) : ((IOrderedQueryable<TEntity>)query).ThenBy(item.Key);
                    }

            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }
    }
}
