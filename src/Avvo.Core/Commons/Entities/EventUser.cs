using System;

namespace Avvo.Core.Commons.Entities;

/// <summary>
/// Representa as informações do usuário associado a um evento.
/// </summary>
public class EventUser
{
    /// <summary>
    /// Obtém o nome do usuário.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Obtém se o usuário está autenticado.
    /// </summary>
    public bool? IsAuthenticated { get; init; }

    /// <summary>
    /// Obtém o tipo de autenticação do usuário.
    /// </summary>
    public string AuthenticationType { get; init; }

    private EventUser(string name, bool? isAuthenticated, string authenticationType)
    {
        Name = name ?? string.Empty;
        IsAuthenticated = isAuthenticated;
        AuthenticationType = authenticationType ?? string.Empty;
    }

    /// <summary>
    /// Cria uma instância de <see cref="EventUser"/>.
    /// </summary>
    /// <param name="name">O nome do usuário.</param>
    /// <param name="isAuthenticated">Indica se o usuário está autenticado.</param>
    /// <param name="authenticationType">O tipo de autenticação.</param>
    /// <returns>Uma instância de <see cref="EventUser"/>.</returns>
    public static EventUser Create(string name, bool? isAuthenticated, string authenticationType)
    {
        return new EventUser(name, isAuthenticated, authenticationType);
    }
}