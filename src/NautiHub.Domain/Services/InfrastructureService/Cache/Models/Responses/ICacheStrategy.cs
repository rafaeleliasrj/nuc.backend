namespace NautiHub.Domain.Services.InfrastructureService.Cache.Models.Responses;

public interface ICacheStrategy
{
    public Task SaveToCache<T>(string cacheKey, T objeto, int timerBufferSegundos);

    public Task<T?> GetCache<T>(string cacheKey);

    public Task RemoveCache(string cacheKey);
}
