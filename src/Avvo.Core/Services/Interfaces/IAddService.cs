using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Services.Interfaces
{
    public interface IAddService<TEntity> where TEntity : class
    {
        Task<TEntity> ExecuteAsync(TEntity entity);
        Task<TEntity> ExecuteTransactionAsync(TEntity entity);
    }
}
