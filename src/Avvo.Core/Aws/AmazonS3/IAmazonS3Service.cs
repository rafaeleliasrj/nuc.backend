using Avvo.Core.Aws.AmazonS3.Dto;

namespace Avvo.Core.Aws.AmazonS3;

public interface IAmazonS3Service
{
    /// <summary>
    /// Faz upload de um arquivo para o Amazon S3.
    /// </summary>
    /// <param name="bucketName">Nome do bucket. Se vazio, usa o bucket padrão configurado.</param>
    /// <param name="filePath">Caminho do arquivo no bucket.</param>
    /// <param name="fileStream">Stream do arquivo a ser enviado.</param>
    /// <param name="contentType">Tipo de conteúdo do arquivo (ex.: "application/pdf").</param>
    /// <param name="fileName">Nome do arquivo. Opcional; se vazio, usa o filePath.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Resultado com a chave do arquivo no bucket.</returns>
    Task<string> UploadFileAsync(string bucketName, string filePath, Stream fileStream, string contentType = "", string fileName = "", CancellationToken ct = default);

    /// <summary>
    /// Exclui um arquivo do Amazon S3.
    /// </summary>
    /// <param name="bucketName">Nome do bucket. Se vazio, usa o bucket padrão configurado.</param>
    /// <param name="filePath">Caminho do arquivo no bucket.</param>
    /// <param name="fileName">Nome do arquivo. Opcional; se vazio, usa o filePath.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Resultado da operação de exclusão.</returns>
    Task<bool> DeleteFileAsync(string bucketName, string filePath, string fileName = "", CancellationToken ct = default);

    /// <summary>
    /// Faz download de um arquivo do Amazon S3 como stream.
    /// </summary>
    /// <param name="bucketName">Nome do bucket. Se vazio, usa o bucket padrão configurado.</param>
    /// <param name="filePath">Caminho do arquivo no bucket.</param>
    /// <param name="fileName">Nome do arquivo. Opcional; se vazio, usa o filePath.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Resultado com o stream do arquivo.</returns>
    Task<AmazonS3FileDto> DownloadFileAsync(string bucketName, string filePath, string fileName = "", CancellationToken ct = default);

    /// <summary>
    /// Lista e faz download de arquivos em uma pasta do Amazon S3 como streams.
    /// </summary>
    /// <param name="bucketName">Nome do bucket. Se vazio, usa o bucket padrão configurado.</param>
    /// <param name="folderPath">Caminho da pasta no bucket.</param>
    /// <param name="maxKeys">Número máximo de arquivos a serem retornados.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Resultado com uma lista de tuplas contendo o nome do arquivo e seu stream.</returns>
    Task<List<(string FileName, AmazonS3FileDto File)>> DownloadFilesByFolderAsync(string bucketName, string folderPath, int maxKeys = 30, CancellationToken ct = default);

    /// <summary>
    /// Gera uma URL pré-assinada para acesso a um arquivo no Amazon S3.
    /// </summary>
    /// <param name="bucketName">Nome do bucket. Se vazio, usa o bucket padrão configurado.</param>
    /// <param name="filePath">Caminho do arquivo no bucket.</param>
    /// <param name="fileName">Nome do arquivo. Opcional; se vazio, usa o filePath.</param>
    /// <param name="expiry">Tempo de expiração da URL. Opcional; se nulo, usa o valor padrão configurado.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Resultado com a URL pré-assinada.</returns>
    Task<string> GetPreSignedUrlAsync(string bucketName, string filePath, string fileName = "", TimeSpan? expiry = null, CancellationToken ct = default);
}
