using System;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Commons.Entities;

/// <summary>
/// Representa a carga de um evento no sistema.
/// </summary>
public class EventPayload
{
    /// <summary>
    /// Obtém o identificador único do evento.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Obtém o ambiente de execução (ex.: produção, desenvolvimento).
    /// </summary>
    public string Landscape { get; init; }

    /// <summary>
    /// Obtém o ambiente específico (ex.: dev, staging).
    /// </summary>
    public string Environment { get; init; }

    /// <summary>
    /// Obtém o nome da aplicação.
    /// </summary>
    public string ApplicationName { get; init; }

    /// <summary>
    /// Obtém a versão da aplicação.
    /// </summary>
    public string ApplicationVersion { get; init; }

    /// <summary>
    /// Obtém a data e hora de início do evento (UTC).
    /// </summary>
    public DateTimeOffset StartDate { get; init; }

    /// <summary>
    /// Obtém a data e hora de término do evento (UTC).
    /// </summary>
    public DateTimeOffset EndDate { get; init; }

    /// <summary>
    /// Obtém a solicitação associada ao evento.
    /// </summary>
    public EventRequest Request { get; init; }

    /// <summary>
    /// Obtém a resposta associada ao evento.
    /// </summary>
    public EventResponse Response { get; init; }

    /// <summary>
    /// Obtém o usuário associado ao evento.
    /// </summary>
    public EventUser User { get; init; }

    /// <summary>
    /// Obtém o requisitante do evento.
    /// </summary>
    public EventRequester Requester { get; init; }

    /// <summary>
    /// Obtém a exceção associada ao evento, se houver.
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// Obtém o logger para registro de eventos.
    /// </summary>
    public ILogger? Logger { get; init; }

    private EventPayload(
        string id,
        string landscape,
        string environment,
        string applicationName,
        string applicationVersion,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        EventRequest request,
        EventResponse response,
        EventUser user,
        EventRequester requester,
        Exception? exception,
        ILogger? logger)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("O ID do evento não pode ser nulo ou vazio.", nameof(id));

        Id = id;
        Landscape = landscape ?? string.Empty;
        Environment = environment ?? string.Empty;
        ApplicationName = applicationName ?? string.Empty;
        ApplicationVersion = applicationVersion ?? string.Empty;
        StartDate = startDate;
        EndDate = endDate;
        Request = request;
        Response = response;
        User = user;
        Requester = requester;
        Exception = exception;
        Logger = logger;
    }

    /// <summary>
    /// Cria uma instância de <see cref="EventPayload"/>.
    /// </summary>
    /// <param name="id">O ID do evento.</param>
    /// <param name="landscape">O ambiente de execução.</param>
    /// <param name="environment">O ambiente específico.</param>
    /// <param name="applicationName">O nome da aplicação.</param>
    /// <param name="applicationVersion">A versão da aplicação.</param>
    /// <param name="startDate">A data de início do evento (UTC).</param>
    /// <param name="endDate">A data de término do evento (UTC).</param>
    /// <param name="request">A solicitação do evento.</param>
    /// <param name="response">A resposta do evento.</param>
    /// <param name="user">O usuário do evento.</param>
    /// <param name="requester">O requisitante do evento.</param>
    /// <param name="exception">A exceção associada, se houver.</param>
    /// <param name="logger">O logger para registro.</param>
    /// <returns>Uma instância de <see cref="EventPayload"/>.</returns>
    public static EventPayload Create(
        string id,
        string landscape,
        string environment,
        string applicationName,
        string applicationVersion,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        EventRequest request,
        EventResponse response,
        EventUser user,
        EventRequester requester,
        Exception? exception,
        ILogger? logger)
    {
        return new EventPayload(id, landscape, environment, applicationName, applicationVersion, startDate, endDate, request, response, user, requester, exception, logger);
    }
}