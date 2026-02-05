namespace NautiHub.Domain.Services.InfrastructureService.Cache;

public interface ICacheService
{
    public Task SaveAsync<T>(string cacheKey, T objeto, int timerBufferSegundos);

    public Task<T?> GetAsync<T>(string cacheKey);

    public Task DeleteAsync(string cacheKey);
}
