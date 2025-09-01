using System.Threading.Tasks;
using Avvo.Core.Data.Pagination;

namespace Avvo.Core.Services.Interfaces
{
    public interface IGetAllPagedService<TEntity, TFilter> where TEntity : class where TFilter : class
    {
        Task<PagedResult<TEntity>> ExecuteAsync(TFilter filter, int page, int pageSize, string sorting);
    }
}
