namespace Avvo.Core.Configuration.Interfaces;

/// <summary>
/// Define o gerenciador de configurações.
/// </summary>
public interface IConfigurationManager
{
    /// <summary>
    /// Adiciona um provedor de configuração.
    /// </summary>
    /// <param name="provider">O provedor de configuração a adicionar.</param>
    void AddProvider(IConfigurationProvider provider);

    /// <summary>
    /// Carrega configurações de todos os provedores registrados.
    /// </summary>
    /// <param name="profile">O perfil de configuração para validação (opcional).</param>
    /// <returns>Um resultado indicando o sucesso ou falha do carregamento.</returns>
    void Load(object? profile = null);

    /// <summary>
    /// Valida as configurações atuais usando o perfil fornecido.
    /// </summary>
    /// <param name="profile">O perfil de configuração a validar.</param>
    void Validate(object profile);

    /// <summary>
    /// Executa todos os modificadores de configuração registrados.
    /// </summary>
    void ApplyModifiers();
}
