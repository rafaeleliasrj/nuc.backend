using System.Net;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Commons.Exceptions;

public class HttpStatusException : ExceptionBase
{
    public HttpStatusCode StatusCode { get; set; }
    public string? ErrorCode { get; set; }

    public HttpStatusException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public HttpStatusException(HttpStatusCode statusCode, string message, LogLevel logLevel) : base(message, logLevel)
    {
        StatusCode = statusCode;
    }

    public HttpStatusException(HttpStatusCode statusCode, string message, string errorCode) : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    public HttpStatusException(HttpStatusCode statusCode, string message, string errorCode, LogLevel logLevel) : base(message, logLevel)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    public HttpStatusException(HttpStatusCode statusCode,
                               string message,
                               string errorCode,
                               Exception innerException,
                               LogLevel logLevel) : base(message, innerException, logLevel: logLevel)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}
