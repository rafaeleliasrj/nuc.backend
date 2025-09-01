using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avvo.Core.Services.Interfaces
{
    public interface IGetAllService<TEntity, TFilter>
        where TEntity : class
        where TFilter : class
    {
        Task<IEnumerable<TEntity>> ExecuteAsync(TFilter filter, int limit = 100, string sorting = null);
    }
}
