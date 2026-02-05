using Microsoft.Extensions.DependencyInjection;
using NautiHub.CrossCutting.Services.File.Enums;
using NautiHub.CrossCutting.Services.File.Operators.S3Bucket;
using NautiHub.Domain.Services.InfrastructureService.File;
using NautiHub.Domain.Services.InfrastructureService.File.Models.Responses;

namespace NautiHub.CrossCutting.Services.File;

public class FileStrategyFactory : IFileStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly FileEnum _server;

    public FileStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        var servidorEnv = Environment.GetEnvironmentVariable("SERVIDOR_ARQUIVOS");
        _server = Enum.TryParse(servidorEnv, out FileEnum servidor)
            ? servidor
            : FileEnum.AwsS3Bucket;
    }

    public IFileStrategy GetService()
    {
        return _server switch
        {
            FileEnum.AwsS3Bucket => _serviceProvider.GetRequiredService<S3BucketStrategy>(),
            _ => _serviceProvider.GetRequiredService<S3BucketStrategy>(),
        };
    }
}
