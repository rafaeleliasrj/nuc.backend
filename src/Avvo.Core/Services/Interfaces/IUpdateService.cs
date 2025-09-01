using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Services.Interfaces
{
    public interface IUpdateService<TEntity>
        where TEntity : class
    {
        Task<int> ExecuteAsync(object id, TEntity entity);
        Task<int> ExecuteTransactionAsync(object id, TEntity entity);
    }
}
