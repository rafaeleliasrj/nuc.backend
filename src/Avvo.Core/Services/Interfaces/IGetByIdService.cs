using System.Threading.Tasks;

namespace Avvo.Core.Services.Interfaces
{
    public interface IGetByIdService<TEntity>
        where TEntity : class
    {
        Task<TEntity> ExecuteAsync(object id);
    }
}
