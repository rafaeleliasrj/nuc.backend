using System.Collections.Generic;

namespace Avvo.Core.Commons.Entities;

/// <summary>
/// Representa uma solicitação associada a um evento.
/// </summary>
public class EventRequest
{
    /// <summary>
    /// Obtém o método HTTP da solicitação (ex.: GET, POST).
    /// </summary>
    public string Method { get; init; }

    /// <summary>
    /// Obtém o caminho da solicitação.
    /// </summary>
    public string Path { get; init; }

    /// <summary>
    /// Obtém o protocolo da solicitação (ex.: HTTP/1.1).
    /// </summary>
    public string Protocol { get; init; }

    /// <summary>
    /// Obtém a query string da solicitação.
    /// </summary>
    public string QueryString { get; init; }

    /// <summary>
    /// Obtém os cabeçalhos da solicitação.
    /// </summary>
    public IReadOnlyDictionary<string, string> Headers { get; init; }

    /// <summary>
    /// Obtém o corpo da solicitação.
    /// </summary>
    public dynamic Body { get; init; }

    private EventRequest(string method, string path, string protocol, string queryString, IReadOnlyDictionary<string, string> headers, dynamic body)
    {
        Method = method ?? string.Empty;
        Path = path ?? string.Empty;
        Protocol = protocol ?? string.Empty;
        QueryString = queryString ?? string.Empty;
        Headers = headers ?? new Dictionary<string, string>().AsReadOnly();
        Body = body;
    }

    /// <summary>
    /// Cria uma instância de <see cref="EventRequest"/>.
    /// </summary>
    /// <param name="method">O método HTTP.</param>
    /// <param name="path">O caminho da solicitação.</param>
    /// <param name="protocol">O protocolo da solicitação.</param>
    /// <param name="queryString">A query string da solicitação.</param>
    /// <param name="headers">Os cabeçalhos da solicitação.</param>
    /// <param name="body">O corpo da solicitação.</param>
    /// <returns>Uma instância de <see cref="EventRequest"/>.</returns>
    public static EventRequest Create(string method, string path, string protocol, string queryString, IReadOnlyDictionary<string, string> headers, dynamic body)
    {
        return new EventRequest(method, path, protocol, queryString, headers, body);
    }
}