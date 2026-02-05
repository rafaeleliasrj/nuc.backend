using System.Net;

namespace NautiHub.Core.DomainObjects;

public class ForbiddenException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ForbiddenException() => StatusCode = HttpStatusCode.Forbidden;

    public ForbiddenException(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.Forbidden
    )
        : base(message) => StatusCode = statusCode;

    public ForbiddenException(
        string message,
        Exception innerException,
        HttpStatusCode statusCode = HttpStatusCode.Forbidden
    )
        : base(message, innerException) => StatusCode = statusCode;
}
