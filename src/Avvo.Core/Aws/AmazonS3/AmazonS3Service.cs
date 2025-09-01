using System.ComponentModel.DataAnnotations;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Avvo.Core.Aws.AmazonS3.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Avvo.Core.Aws.AmazonS3;

public class AmazonS3Service : IAmazonS3Service, IAsyncDisposable
{
    private readonly IAmazonS3 _s3Client;
    private readonly AmazonS3Options _options;
    private readonly ILogger<AmazonS3Service> _logger;
    private bool _disposed;
    private const string BUCKET_IS_REQUIRED = "O nome do bucket é obrigatório.";
    private const string FILE_PATH_IS_REQUIRED = "O caminho do arquivo é obrigatório.";
    private const string FILE_STREAM_IS_INVALID = "O stream do arquivo é inválido.";
    private const string FOLDER_PATH_IS_REQUIRED = "O caminho da pasta é obrigatório.";
    private const string MAX_KEYS_MUST_BE_GREATER_THAN_ZERO = "O número máximo de chaves deve ser maior que zero.";

    public AmazonS3Service(IAmazonS3 s3Client, IOptions<AmazonS3Options> options, ILogger<AmazonS3Service> logger)
    {
        _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> UploadFileAsync(string bucketName, string filePath, Stream fileStream, string contentType = "", string fileName = "", CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(bucketName))
            throw new ValidationException(BUCKET_IS_REQUIRED);
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ValidationException(FILE_PATH_IS_REQUIRED);
        if (fileStream == null || fileStream.Length == 0)
            throw new ValidationException(FILE_STREAM_IS_INVALID);

        var effectiveBucket = string.IsNullOrEmpty(bucketName) ? _options.DefaultBucket : bucketName;
        var fileKey = BuildFileKey(filePath, fileName);

        try
        {
            if (ConvertBytesToMegabytes(fileStream.Length) > _options.MaxFileSizeMb)
                throw new ValidationException($"O tamanho do arquivo excede o limite de {_options.MaxFileSizeMb} MB.");

            _logger.LogInformation("Iniciando upload do arquivo {FileKey} para o bucket {BucketName}.", fileKey, effectiveBucket);

            using var fileTransferUtility = new TransferUtility(_s3Client);
            var request = new TransferUtilityUploadRequest
            {
                BucketName = effectiveBucket,
                Key = fileKey,
                InputStream = fileStream,
                ContentType = string.IsNullOrEmpty(contentType) ? "application/octet-stream" : contentType,
                CannedACL = S3CannedACL.Private
            };

            await fileTransferUtility.UploadAsync(request, ct);
            _logger.LogInformation("Upload do arquivo {FileKey} concluído com sucesso.", fileKey);
            return fileKey;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer upload do arquivo {FileKey} para o bucket {BucketName}.", fileKey, effectiveBucket);
            throw new ValidationException($"Erro ao fazer upload: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao fazer upload do arquivo {FileKey}.", fileKey);
            throw new ValidationException($"Erro inesperado: {ex.Message}");
        }
    }

    public async Task<bool> DeleteFileAsync(string bucketName, string filePath, string fileName = "", CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(bucketName))
            throw new ValidationException(BUCKET_IS_REQUIRED);
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ValidationException("O caminho do arquivo é obrigatório.");

        var effectiveBucket = string.IsNullOrEmpty(bucketName) ? _options.DefaultBucket : bucketName;
        var fileKey = BuildFileKey(filePath, fileName);

        try
        {
            _logger.LogInformation("Iniciando exclusão do arquivo {FileKey} no bucket {BucketName}.", fileKey, effectiveBucket);

            var request = new DeleteObjectRequest
            {
                BucketName = effectiveBucket,
                Key = fileKey
            };

            await _s3Client.DeleteObjectAsync(request, ct);
            _logger.LogInformation("Arquivo {FileKey} excluído com sucesso.", fileKey);
            return true;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir o arquivo {FileKey} do bucket {BucketName}.", fileKey, effectiveBucket);
            throw new ValidationException($"Erro ao excluir: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao excluir o arquivo {FileKey}.", fileKey);
            throw new ValidationException($"Erro inesperado: {ex.Message}");
        }
    }

    public async Task<AmazonS3FileDto> DownloadFileAsync(string bucketName, string filePath, string fileName = "", CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(bucketName))
            throw new ValidationException(BUCKET_IS_REQUIRED);
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ValidationException(FILE_PATH_IS_REQUIRED);

        var effectiveBucket = string.IsNullOrEmpty(bucketName) ? _options.DefaultBucket : bucketName;
        var fileKey = BuildFileKey(filePath, fileName);

        try
        {
            _logger.LogInformation("Iniciando download do arquivo {FileKey} do bucket {BucketName}.", fileKey, effectiveBucket);

            var request = new GetObjectRequest
            {
                BucketName = effectiveBucket,
                Key = fileKey
            };

            using var response = await _s3Client.GetObjectAsync(request, ct);
            _logger.LogInformation("Download do arquivo {FileKey} concluído com sucesso.", fileKey);

            var amazonS3Dto = new AmazonS3FileDto
            (
                fileName: response.Key,
                fileStream: response.ResponseStream,
                contentType: response.Headers.ContentType,
                contentLength: response.ContentLength,
                lastModified: response.LastModified
            );

            return amazonS3Dto;
        }
        catch (AmazonS3Exception ex) when (ex.ErrorCode.Equals("NoSuchKey", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Arquivo {FileKey} não encontrado no bucket {BucketName}.", fileKey, effectiveBucket);
            throw new ValidationException("Arquivo não encontrado.");
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer download do arquivo {FileKey} do bucket {BucketName}.", fileKey, effectiveBucket);
            throw new ValidationException($"Erro ao fazer download: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao fazer download do arquivo {FileKey}.", fileKey);
            throw new ValidationException($"Erro inesperado: {ex.Message}");
        }
    }

    public async Task<List<(string FileName, AmazonS3FileDto File)>> DownloadFilesByFolderAsync(string bucketName, string folderPath, int maxKeys = 30, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(bucketName))
            throw new ValidationException(BUCKET_IS_REQUIRED);
        if (string.IsNullOrWhiteSpace(folderPath))
            throw new ValidationException(FOLDER_PATH_IS_REQUIRED);
        if (maxKeys < 1)
            throw new ValidationException(MAX_KEYS_MUST_BE_GREATER_THAN_ZERO);

        var effectiveBucket = string.IsNullOrEmpty(bucketName) ? _options.DefaultBucket : bucketName;
        var results = new List<(string FileName, AmazonS3FileDto File)>();

        try
        {
            _logger.LogInformation("Listando arquivos na pasta {FolderPath} do bucket {BucketName}.", folderPath, effectiveBucket);

            var listRequest = new ListObjectsV2Request
            {
                BucketName = effectiveBucket,
                Prefix = folderPath,
                Delimiter = "/",
                MaxKeys = maxKeys
            };

            var listResponse = await _s3Client.ListObjectsV2Async(listRequest, ct);
            foreach (var s3Object in listResponse.S3Objects)
            {
                var getRequest = new GetObjectRequest
                {
                    BucketName = effectiveBucket,
                    Key = s3Object.Key
                };

                try
                {
                    using var getResponse = await _s3Client.GetObjectAsync(getRequest, ct);

                    var amazonS3Dto = new AmazonS3FileDto(
                        fileName: s3Object.Key,
                        fileStream: getResponse.ResponseStream,
                        contentType: getResponse.Headers.ContentType,
                        contentLength: getResponse.ContentLength,
                        lastModified: getResponse.LastModified
                    );

                    results.Add((s3Object.Key, amazonS3Dto));
                }
                catch (AmazonS3Exception ex) when (ex.ErrorCode.Equals("NoSuchKey", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Arquivo {FileKey} não encontrado durante listagem.", s3Object.Key);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Erro ao baixar o arquivo {FileKey}.", s3Object.Key);
                }
            }

            _logger.LogInformation("Listagem de arquivos na pasta {FolderPath} concluída com {Count} arquivos.", folderPath, results.Count);
            return results;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar arquivos na pasta {FolderPath} do bucket {BucketName}.", folderPath, effectiveBucket);
            throw new ValidationException($"Erro ao listar arquivos: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao listar arquivos na pasta {FolderPath}.", folderPath);
            throw new ValidationException($"Erro inesperado: {ex.Message}");
        }
    }

    public async Task<string> GetPreSignedUrlAsync(string bucketName, string filePath, string fileName = "", TimeSpan? expiry = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(bucketName))
            throw new ValidationException(BUCKET_IS_REQUIRED);
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ValidationException(FILE_PATH_IS_REQUIRED);

        var effectiveBucket = string.IsNullOrEmpty(bucketName) ? _options.DefaultBucket : bucketName;
        var fileKey = BuildFileKey(filePath, fileName);
        var effectiveExpiry = expiry ?? _options.PreSignedUrlExpiry;

        try
        {
            _logger.LogInformation("Gerando URL pré-assinada para o arquivo {FileKey} no bucket {BucketName}.", fileKey, effectiveBucket);

            var request = new GetPreSignedUrlRequest
            {
                BucketName = effectiveBucket,
                Key = fileKey,
                Expires = DateTime.UtcNow.Add(effectiveExpiry),
                Protocol = Protocol.HTTPS
            };

            var url = await _s3Client.GetPreSignedURLAsync(request);
            _logger.LogInformation("URL pré-assinada gerada com sucesso para o arquivo {FileKey}.", fileKey);
            return url;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar URL pré-assinada para o arquivo {FileKey}.", fileKey);
            throw new ValidationException($"Erro ao gerar URL: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao gerar URL pré-assinada para o arquivo {FileKey}.", fileKey);
            throw new ValidationException($"Erro inesperado: {ex.Message}");
        }
    }

    private static string BuildFileKey(string filePath, string fileName)
    {
        filePath = filePath.Trim('/');
        return string.IsNullOrEmpty(fileName) ? filePath : $"{filePath}/{fileName}".Trim('/');
    }

    private static double ConvertBytesToMegabytes(long bytes)
    {
        return bytes / 1024.0 / 1024.0;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        if (_s3Client is IAsyncDisposable asyncDisposable)
            await asyncDisposable.DisposeAsync();
        else
            _s3Client.Dispose();

        _disposed = true;
    }
}
