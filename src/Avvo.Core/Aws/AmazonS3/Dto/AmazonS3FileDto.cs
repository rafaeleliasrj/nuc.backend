using System;
using System.IO;

namespace Avvo.Core.Aws.AmazonS3.Dto;

public class AmazonS3FileDto : IDisposable
{
    public string FileName { get; init; }
    public Stream FileStream { get; init; }
    public string ContentType { get; init; }
    public long ContentLength { get; init; }
    public DateTime? LastModified { get; init; }

    public AmazonS3FileDto(string fileName, Stream fileStream, string contentType, long contentLength, DateTime? lastModified)
    {
        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        FileStream = fileStream ?? throw new ArgumentNullException(nameof(fileStream));
        ContentType = contentType ?? string.Empty;
        ContentLength = contentLength;
        LastModified = lastModified;
    }

    public void Dispose()
    {
        FileStream?.Dispose();
        GC.SuppressFinalize(this);
    }
}
