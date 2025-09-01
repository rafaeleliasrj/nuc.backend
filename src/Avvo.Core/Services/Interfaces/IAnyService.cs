using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avvo.Core.Services.Interfaces
{
    public interface IAnyService<TEntity, TFilter>
        where TEntity : class
        where TFilter : class
    {
        Task<bool> ExecuteAsync(TFilter filter);
    }
}
