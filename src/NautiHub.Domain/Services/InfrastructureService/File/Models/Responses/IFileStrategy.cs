using NautiHub.Domain.Services.InfrastructureService.File.Models.Requests;

namespace NautiHub.Domain.Services.InfrastructureService.File.Models.Responses;

public interface IFileStrategy
{
    public Task<string> SaveAsync(FileRequest file);

    public Task<byte[]> FindAsync(string url);

    public Task DeleteAsync(string url);
}
