using NautiHub.Core.DomainObjects;

namespace NautiHub.Core.Data;

public interface IRepository<T> : IDisposable
    where T : IAggregateRoot
{
    public Task AddAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task DeleteAsync(T entity);
}
