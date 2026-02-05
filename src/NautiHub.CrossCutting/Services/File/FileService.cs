using Microsoft.Extensions.Logging;
using NautiHub.Domain.Services.InfrastructureService.File;
using NautiHub.Domain.Services.InfrastructureService.File.Models.Requests;
using NautiHub.Domain.Services.InfrastructureService.File.Models.Responses;

namespace NautiHub.Infrastructure.Services.File;

public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;
    private readonly IFileStrategyFactory _strategyFactory;

    public FileService(ILogger<FileService> logger,
                          IFileStrategyFactory strategyFactory)
    {
        _logger = logger;
        _strategyFactory = strategyFactory;
    }

    public async Task<string> SaveAsync(FileRequest arquivo)
    {
        IFileStrategy strategy = _strategyFactory.GetService();
        return await strategy.SaveAsync(arquivo);
    }

    public async Task<byte[]> FindAsync(string urlArquivo)
    {
        IFileStrategy strategy = _strategyFactory.GetService();
        return await strategy.FindAsync(urlArquivo);
    }

    public async Task DeleteAsync(string urlArquivo)
    {
        IFileStrategy strategy = _strategyFactory.GetService();
        await strategy.DeleteAsync(urlArquivo);
    }
}
