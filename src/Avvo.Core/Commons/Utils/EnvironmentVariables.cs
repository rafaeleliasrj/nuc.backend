using System.Net;
using Avvo.Core.Commons.Exceptions;

namespace Avvo.Core.Commons.Utils;

/// <summary>
/// Fornece métodos para gerenciar variáveis de ambiente no escopo do processo atual.
/// </summary>
public static class EnvironmentVariables
{
    private const string ENVIRONMENT_NOT_NULL_OR_EMPTY = "O nome da variável de ambiente não pode ser nulo ou vazio.";
    /// <summary>
    /// Define uma variável de ambiente para o processo atual.
    /// </summary>
    /// <param name="name">O nome único da variável de ambiente.</param>
    /// <param name="value">O valor da variável de ambiente.</param>
    /// <exception cref="ArgumentException">Lançada se o nome for nulo ou vazio.</exception>
    public static void Set(string? name, string? value)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(ENVIRONMENT_NOT_NULL_OR_EMPTY, nameof(name));

        Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Process);
    }

    /// <summary>
    /// Define uma variável de ambiente para o processo atual apenas se ela ainda não estiver definida.
    /// </summary>
    /// <param name="name">O nome único da variável de ambiente.</param>
    /// <param name="value">O valor da variável de ambiente.</param>
    /// <exception cref="ArgumentException">Lançada se o nome for nulo ou vazio.</exception>
    public static void SetIfNull(string? name, string? value)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(ENVIRONMENT_NOT_NULL_OR_EMPTY, nameof(name));

        if (Get(name) != null)
            return;

        Set(name, value);
    }

    /// <summary>
    /// Obtém o valor de uma variável de ambiente do processo atual.
    /// </summary>
    /// <param name="name">O nome único da variável de ambiente.</param>
    /// <returns>Um resultado com o valor da variável ou um erro se ela não for encontrada.</returns>
    /// <exception cref="ArgumentException">Lançada se o nome for nulo ou vazio.</exception>
    public static string Get(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new HttpStatusException(HttpStatusCode.BadRequest, "O nome da variável de ambiente não pode ser nulo ou vazio.", "E400");

        var value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        if (value == null)
            throw new HttpStatusException(HttpStatusCode.NotFound, $"Variável de ambiente '{name}' não encontrada.", "E404");

        return value;
    }

    /// <summary>
    /// Obtém o valor de uma variável de ambiente do processo atual ou um valor padrão se ela não for encontrada.
    /// </summary>
    /// <param name="name">O nome único da variável de ambiente.</param>
    /// <param name="defaultValue">O valor padrão a ser retornado se a variável não for encontrada.</param>
    /// <returns>O valor da variável ou o valor padrão.</returns>
    /// <exception cref="ArgumentException">Lançada se o nome for nulo ou vazio.</exception>
    public static string GetOrDefault(string? name, string defaultValue)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da variável de ambiente não pode ser nulo ou vazio.", nameof(name));

        return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process) ?? defaultValue;
    }

    /// <summary>
    /// Remove uma variável de ambiente do processo atual.
    /// </summary>
    /// <param name="name">O nome único da variável de ambiente.</param>
    /// <exception cref="ArgumentException">Lançada se o nome for nulo ou vazio.</exception>
    public static void Delete(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da variável de ambiente não pode ser nulo ou vazio.", nameof(name));

        Environment.SetEnvironmentVariable(name, null, EnvironmentVariableTarget.Process);
    }
}
