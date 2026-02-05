using Microsoft.EntityFrameworkCore;
using NautiHub.Core.DomainObjects;

namespace NautiHub.Core.Data;

public class Repository<TEntity, TDbContext> : IRepository<TEntity>
    where TEntity : Entity, IAggregateRoot
    where TDbContext : DbContext, IUnitOfWork
{
    protected internal readonly TDbContext _context;

    protected internal readonly DbSet<TEntity> _dbSet;

    public Repository(TDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public Task AddAsync(TEntity entity)
    {
        _context.Add(entity);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public Task UpdateAsync(TEntity entity)
    {
        _context.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        _context.Remove(entity);
        return Task.CompletedTask;
    }
}
