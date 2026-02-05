namespace NautiHub.Core.Data;

public interface IUnitOfWork
{
    public Task CommitAsync(bool ativarSoftDelete = true);
}
