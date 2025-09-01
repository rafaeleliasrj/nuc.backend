namespace Avvo.Core.Configuration.Interfaces;

/// <summary>
/// Define um modificador de configuração.
/// </summary>
public interface IConfigurationModifier
{
    /// <summary>
    /// Determina se o modificador é válido para execução.
    /// </summary>
    /// <returns><c>true</c> se o modificador for válido; caso contrário, <c>false</c>.</returns>
    bool IsValid();

    /// <summary>
    /// Executa o modificador de configuração.
    /// </summary>
    void Execute();
}
