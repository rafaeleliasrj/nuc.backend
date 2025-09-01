using System.Threading.Tasks;

namespace Avvo.Core.Services.Interfaces
{
    public interface IValidateService<TEntity>
        where TEntity : class
    {
        Task ExecuteAsync(TEntity entity);
    }
}
