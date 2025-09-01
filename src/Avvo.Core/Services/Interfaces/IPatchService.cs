using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Services.Interfaces
{
    public interface IPatchService<TEntity>
        where TEntity : class
    {
        Task<int> ExecuteAsync(object id, JsonPatchDocument<TEntity> attributes);
        Task<int> ExecuteTransactionAsync(object id, JsonPatchDocument<TEntity> attributes);
        Task<(TEntity entity, int numEntitiesChanged)> ExecuteWithReturnEntityAsync(object id, JsonPatchDocument<TEntity> attributes);
        Task<(TEntity entity, int numEntitiesChanged)> ExecuteTransactionWithReturnEntityAsync(object id, JsonPatchDocument<TEntity> attributes);
    }
}
