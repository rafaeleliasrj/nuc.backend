using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Data.Interfaces
{
    public interface IUpdateRepository<TEntity> where TEntity : class
    {
        Task<int> ExecuteAsync(DbContext dbContext, object id, TEntity entity);
    }
}
