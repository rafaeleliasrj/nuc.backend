using System;

namespace Avvo.Core.Commons.Entities;

/// <summary>
/// Representa a resposta associada a um evento.
/// </summary>
public class EventResponse
{
    /// <summary>
    /// Obtém o tempo de resposta em milissegundos.
    /// </summary>
    public double ResponseTime { get; init; }

    /// <summary>
    /// Obtém o código de status HTTP da resposta.
    /// </summary>
    public int? StatusCode { get; init; }

    /// <summary>
    /// Obtém o corpo da resposta.
    /// </summary>
    public object Body { get; init; }

    private EventResponse(double responseTime, int? statusCode, object body)
    {
        if (responseTime < 0)
            throw new ArgumentException("O tempo de resposta não pode ser negativo.", nameof(responseTime));

        ResponseTime = responseTime;
        StatusCode = statusCode;
        Body = body;
    }

    /// <summary>
    /// Cria uma instância de <see cref="EventResponse"/>.
    /// </summary>
    /// <param name="responseTime">O tempo de resposta em milissegundos.</param>
    /// <param name="statusCode">O código de status HTTP.</param>
    /// <param name="body">O corpo da resposta.</param>
    /// <returns>Uma instância de <see cref="EventResponse"/>.</returns>
    public static EventResponse Create(double responseTime, int? statusCode, object body)
    {
        return new EventResponse(responseTime, statusCode, body);
    }
}