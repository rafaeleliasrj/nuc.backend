using System.Net;
using System.Text.Json;
using Avvo.Core.Commons.Exceptions;

namespace Avvo.Core.Commons.Entities;

/// <summary>
/// Representa os detalhes de um erro ocorrido no sistema.
/// </summary>
public class ErrorDetails
{
    public ErrorDetails()
    {
        Messages = new List<string>();
    }

    /// <summary>
    /// Obtém o código de status HTTP do erro.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Obtém o código interno do erro.
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Obtém a lista de mensagens de erro.
    /// </summary>
    public List<string> Messages { get; set; }

    /// <summary>
    /// Serializa o objeto para JSON.
    /// </summary>
    /// <returns>Uma representação JSON do objeto.</returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    /// <summary>
    /// Cria uma instância vazia de <see cref="ErrorDetails"/>.
    /// </summary>
    /// <returns>Um resultado com uma instância de <see cref="ErrorDetails"/> vazia.</returns>
    public static ErrorDetails Create() => new ErrorDetails { };

    /// <summary>
    /// Cria uma instância de <see cref="ErrorDetails"/> com os valores especificados.
    /// </summary>
    /// <param name="statusCode">O código de status HTTP.</param>
    /// <param name="errorCode">O código interno do erro.</param>
    /// <param name="messages">A lista de mensagens de erro.</param>
    /// <returns>Um resultado com a instância de <see cref="ErrorDetails"/>.</returns>
    public static ErrorDetails Create(HttpStatusCode statusCode, string errorCode, List<string> messages) => new ErrorDetails
    {
        StatusCode = statusCode,
        ErrorCode = errorCode,
        Messages = messages
    };

    /// <summary>
    /// Adiciona uma mensagem à lista de mensagens de erro.
    /// </summary>
    /// <param name="message">A mensagem a ser adicionada.</param>
    /// <returns>Um novo <see cref="ErrorDetails"/> com a mensagem adicionada.</returns>
    public void AddMessage(string message)
    {
        Messages.Add(message);
    }
}
