using System;

namespace Avvo.Core.Configuration;

/// <summary>
/// Exceção lançada quando a validação de configuração falha.
/// </summary>
public class ConfigurationValidationException : Exception
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="ConfigurationValidationException"/>.
    /// </summary>
    /// <param name="message">A mensagem da exceção.</param>
    public ConfigurationValidationException(string message) : base(message) { }
}
