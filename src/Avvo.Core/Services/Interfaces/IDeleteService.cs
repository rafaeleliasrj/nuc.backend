using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Services.Interfaces
{
    public interface IDeleteService<TEntity> where TEntity : class
    {
        Task<int> ExecuteAsync(object id, bool propagateEvent = false, List<int> propagateDestination = null, string overrideEntityName = null);
        Task<int> ExecuteTransactionAsync(object id, bool propagateEvent = false, List<int> propagateDestination = null, string overrideEntityName = null);
    }
}
