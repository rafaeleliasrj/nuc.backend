using Microsoft.AspNetCore.Http;

namespace NautiHub.Core.Extensions;

public static class HttpContextExtensions
{
    private const string HeaderName = "X-Request-Id";

    public static Guid GetRequestId(this HttpContext context)
    {
        return context.Request.Headers.TryGetValue(HeaderName, out Microsoft.Extensions.Primitives.StringValues value) &&
               Guid.TryParse(value, out Guid parsed)
            ? parsed
            : Guid.NewGuid();
    }
}
