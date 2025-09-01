namespace Avvo.Core.Configuration;

/// <summary>
/// Especifica o formato de um parâmetro de configuração.
/// </summary>
public enum ParameterFormat
{
    /// <summary>
    /// O conteúdo do parâmetro é YAML.
    /// </summary>
    Yaml,

    /// <summary>
    /// O conteúdo do parâmetro é texto bruto.
    /// </summary>
    Raw,

    /// <summary>
    /// O conteúdo do parâmetro é JSON.
    /// </summary>
    Json
}
