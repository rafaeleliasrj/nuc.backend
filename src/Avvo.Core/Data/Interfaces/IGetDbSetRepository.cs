using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Data.Interfaces
{
    public interface IGetDbSetRepository<TEntity> where TEntity : class
    {
        DbSet<TEntity> Execute(DbContext dbContext);
    }
}
