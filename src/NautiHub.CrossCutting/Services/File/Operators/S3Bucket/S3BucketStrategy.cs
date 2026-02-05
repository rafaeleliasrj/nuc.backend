using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Logging;
using NautiHub.Core.Aws;
using NautiHub.Domain.Services.InfrastructureService.File.Models.Enums;
using NautiHub.Domain.Services.InfrastructureService.File.Models.Requests;
using NautiHub.Domain.Services.InfrastructureService.File.Models.Responses;
using NautiHub.Infrastructure.Services.File;

namespace NautiHub.CrossCutting.Services.File.Operators.S3Bucket;

public class S3BucketStrategy(ILogger<FileService> logger,
                                         IClientAws apiClientAws,
                                         IAmazonS3 clientAws) : IFileStrategy
{
    private readonly ILogger<FileService> _logger = logger;
    private readonly string _bucketName = Environment.GetEnvironmentVariable("AWS_S3_BUCKET_NAME") ?? throw new Exception("Variável de ambiente AWS_S3_BUCKET_NAME não informada.");
    private readonly string _bucketPath = Environment.GetEnvironmentVariable("AWS_S3_BUCKET_PATH") ?? string.Empty;
    private readonly string _awsRegion = apiClientAws.GetAwsRegion();
    private readonly string? _localstackEndpoint = Environment.GetEnvironmentVariable("LOCALSTACK_ENDPOINT");
    private readonly IAmazonS3 _clientAws = clientAws;

    private const string UrlBucketPrivadoTemplate = "https://{0}.s3.{1}.amazonaws.com/";
    private const string UrlBucketPublicoTemplate = "https://s3.{0}.amazonaws.com/{1}/";

    public async Task DeleteAsync(string urlArquivo)
    {
        try
        {
            _logger.LogInformation("[ApagarArquivo] - [urlArquivo: {urlArquivo}] - Iniciando processo de deletar arquivo no bucket s3.", urlArquivo);

            var key = ExtrairKey(urlArquivo);

            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("Url de arquivo inválida!");
            }

            var deleteObjectsRequest = new DeleteObjectsRequest
            {
                BucketName = _bucketName,
                Objects = [new() { Key = key }]
            };

            await _clientAws.DeleteObjectsAsync(deleteObjectsRequest);
            _logger.LogInformation("[ApagarArquivo] - [urlArquivo: {urlArquivo}] - Arquivo deletado com sucesso.", urlArquivo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ApagarArquivo] - [urlArquivo: {urlArquivo}] - Erro ao deletar arquivo do s3.", urlArquivo);
            throw;
        }
    }

    public async Task<byte[]> FindAsync(string urlArquivo)
    {
        try
        {
            var key = ExtrairKey(urlArquivo);

            _logger.LogInformation("[BuscarArquivo] - [urlArquivo: {urlArquivo}] - Iniciando download do arquivo no bucket s3.", urlArquivo);

            if (string.IsNullOrEmpty(key))
            {
                _logger.LogInformation("[BuscarArquivo] - [urlArquivo: {urlArquivo}] - Url de arquivo inválida.", urlArquivo);
                throw new Exception("Url de arquivo inválida!");
            }

            using GetObjectResponse retornoStream = await _clientAws.GetObjectAsync(_bucketName, key);
            using var ms = new MemoryStream();
            _logger.LogInformation("[BuscarArquivo] - [urlArquivo: {urlArquivo}] - Convertendo memorystream para array de byte.", urlArquivo);
            await retornoStream.ResponseStream.CopyToAsync(ms);
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[BuscarArquivo] - [urlArquivo: {urlArquivo}] - Erro ao baixar arquivo do s3.", urlArquivo);
            throw;
        }
    }

    public async Task<string> SaveAsync(FileRequest arquivo)
    {
        try
        {
            _logger.LogInformation("[SalvarArquivo] - [Nome do arquivo: {Nome}, Bucket: {_bucketName}] - Iniciando envio de arquivo para o servidor de arquivos s3 bucket aws.", arquivo.Name, _bucketName);
            S3CannedACL visibilidade = arquivo.Disponibilidade == FileVisibilityEnum.Private ? S3CannedACL.Private : S3CannedACL.PublicRead;

            var nomeArquivo = string.IsNullOrEmpty(_bucketPath) ? arquivo.Name : $"{_bucketPath}/{arquivo.Name}";

            using (var inputStream = new MemoryStream(arquivo.Content))
            {
                var request = new TransferUtilityUploadRequest
                {
                    BucketName = _bucketName,
                    Key = nomeArquivo,
                    InputStream = inputStream,
                    CannedACL = visibilidade
                };

                if (arquivo.ExpireIn.HasValue)
                    request.TagSet = new List<Tag> { new Tag { Key = "ExpireAfter", Value = arquivo.ExpireIn.Value.ToString() } };

                var utility = new TransferUtility(_clientAws);
                await utility.UploadAsync(request);
            }

            var urlBucket = MontarUrlArquivo(nomeArquivo, arquivo.Disponibilidade);

            _logger.LogInformation("[SalvarArquivo] - [Nome do arquivo: {Nome}] - Arquivo salvo no s3 bucket aws com sucesso. URL: {urlBucket}", arquivo.Name, urlBucket);
            return urlBucket;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SalvarArquivo] - [Nome do arquivo: {Nome}] - Erro ao enviar arquivo para s3 bucket aws.", arquivo.Name);
            throw;
        }
    }


    private string MontarUrlArquivo(string nomeArquivo, FileVisibilityEnum disponibilidade)
    {
        if (!string.IsNullOrEmpty(_localstackEndpoint))
            return $"{_localstackEndpoint}/{_bucketName}/{nomeArquivo}";

        return disponibilidade == FileVisibilityEnum.Private
                ? string.Format(UrlBucketPrivadoTemplate, _bucketName, _awsRegion) + nomeArquivo
                : string.Format(UrlBucketPublicoTemplate, _awsRegion, _bucketName) + nomeArquivo;
    }

    private string ExtrairKey(string urlArquivo)
    {
        if (!string.IsNullOrEmpty(_localstackEndpoint))
            return urlArquivo.Replace($"{_localstackEndpoint}/{_bucketName}/", "");

        var urlBucketPrivado = string.Format(UrlBucketPrivadoTemplate, _bucketName, _awsRegion);
        var urlBucketPublico = string.Format(UrlBucketPublicoTemplate, _awsRegion, _bucketName);

        return urlArquivo
            .Replace(urlBucketPrivado, "")
            .Replace(urlBucketPublico, "");
    }
}
