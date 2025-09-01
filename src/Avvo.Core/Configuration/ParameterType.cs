namespace Avvo.Core.Configuration;

/// <summary>
/// Define o tipo de um parâmetro de configuração.
/// </summary>
public enum ParameterType
{
    /// <summary>
    /// Parâmetro compartilhado globalmente, não específico a uma aplicação.
    /// </summary>
    Global,

    /// <summary>
    /// Parâmetro específico de uma aplicação.
    /// </summary>
    Application
}
