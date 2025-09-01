using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Data.Interfaces
{
    public interface IAddRepository<TEntity> where TEntity : class
    {
        Task<TEntity> ExecuteAsync(DbContext dbContext, TEntity entity);
    }
}
