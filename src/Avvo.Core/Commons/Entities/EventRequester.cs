using System;

namespace Avvo.Core.Commons.Entities;

/// <summary>
/// Representa o requisitante de um evento.
/// </summary>
public class EventRequester
{
    /// <summary>
    /// Obtém a chave de API do requisitante.
    /// </summary>
    public string ApiKey { get; init; }

    /// <summary>
    /// Obtém o token de acesso do requisitante.
    /// </summary>
    public string AccessToken { get; init; }

    /// <summary>
    /// Obtém o ambiente de execução do requisitante.
    /// </summary>
    public string Landscape { get; init; }

    /// <summary>
    /// Obtém o ambiente específico do requisitante.
    /// </summary>
    public string Environment { get; init; }

    /// <summary>
    /// Obtém o nome da aplicação do requisitante.
    /// </summary>
    public string ApplicationName { get; init; }

    /// <summary>
    /// Obtém a versão da aplicação do requisitante.
    /// </summary>
    public string ApplicationVersion { get; init; }

    /// <summary>
    /// Obtém o endereço IP do requisitante.
    /// </summary>
    public string Ip { get; init; }

    private EventRequester(string apiKey, string accessToken, string landscape, string environment, string applicationName, string applicationVersion, string ip)
    {
        ApiKey = apiKey ?? string.Empty;
        AccessToken = accessToken ?? string.Empty;
        Landscape = landscape ?? string.Empty;
        Environment = environment ?? string.Empty;
        ApplicationName = applicationName ?? string.Empty;
        ApplicationVersion = applicationVersion ?? string.Empty;
        Ip = ip ?? string.Empty;
    }

    /// <summary>
    /// Cria uma instância de <see cref="EventRequester"/>.
    /// </summary>
    /// <param name="apiKey">A chave de API.</param>
    /// <param name="accessToken">O token de acesso.</param>
    /// <param name="landscape">O ambiente de execução.</param>
    /// <param name="environment">O ambiente específico.</param>
    /// <param name="applicationName">O nome da aplicação.</param>
    /// <param name="applicationVersion">A versão da aplicação.</param>
    /// <param name="ip">O endereço IP.</param>
    /// <returns>Uma instância de <see cref="EventRequester"/>.</returns>
    public static EventRequester Create(string apiKey, string accessToken, string landscape, string environment, string applicationName, string applicationVersion, string ip)
    {
        return new EventRequester(apiKey, accessToken, landscape, environment, applicationName, applicationVersion, ip);
    }
}