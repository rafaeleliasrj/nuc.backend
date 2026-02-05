namespace NautiHub.Common.Middlewares;

/// <summary>
/// Middleware responsável por definir um identificador único para cada requisição HTTP.
/// </summary>
public class RequestIdMiddleware
{
    private const string HeaderName = "X-Request-Id";
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestIdMiddleware> _logger;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="RequestIdMiddleware"/>.
    /// </summary>
    /// <param name="next">Delegado para invocar o próximo middleware na pipeline.</param>
    /// <param name="logger">Instância de <see cref="ILogger{RequestIdMiddleware}"/> para registrar logs.</param>
    public RequestIdMiddleware(RequestDelegate next, ILogger<RequestIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Método invocado para processar a requisição HTTP e definir o identificador único.
    /// </summary>
    /// <param name="context">Contexto da requisição HTTP.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        Guid requestId = context.Request.Headers.TryGetValue(HeaderName, out Microsoft.Extensions.Primitives.StringValues headerValue) &&
                        Guid.TryParse(headerValue, out Guid parsed)
            ? parsed
            : Guid.NewGuid();

        context.Items[HeaderName] = requestId;

        _logger.LogDebug("RequestId definido como: {RequestId}", requestId);

        await _next(context);
    }
}
