using System;

namespace Avvo.Core.Configuration;

/// <summary>
/// Define um parâmetro de provedor de configuração.
/// </summary>
public class ConfigurationProviderParameter
{
    /// <summary>
    /// Obtém o nome do parâmetro.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Obtém o formato do conteúdo do parâmetro (Yaml, Json ou Raw).
    /// </summary>
    public ParameterFormat Format { get; init; }

    /// <summary>
    /// Obtém o tipo do parâmetro (Global ou Application).
    /// </summary>
    public ParameterType Type { get; init; }

    /// <summary>
    /// Obtém ou define o valor do parâmetro.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ConfigurationProviderParameter"/>.
    /// </summary>
    /// <param name="name">O nome do parâmetro.</param>
    /// <param name="format">O formato do parâmetro (padrão: Yaml).</param>
    /// <param name="type">O tipo do parâmetro (padrão: Application).</param>
    /// <exception cref="ArgumentException">Lançada se o nome for nulo ou vazio.</exception>
    public ConfigurationProviderParameter(string name, ParameterFormat format = ParameterFormat.Yaml, ParameterType type = ParameterType.Application)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do parâmetro não pode ser nulo ou vazio.", nameof(name));

        Name = name;
        Format = format;
        Type = type;
        Value = null;
    }
}
