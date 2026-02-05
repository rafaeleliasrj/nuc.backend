using System.Net;

namespace NautiHub.Core.DomainObjects;

public class DomainException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public DomainException() => StatusCode = HttpStatusCode.BadRequest;

    public DomainException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message) => StatusCode = statusCode;

    public DomainException(
        string message,
        Exception innerException,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest
    )
        : base(message, innerException) => StatusCode = statusCode;
}
