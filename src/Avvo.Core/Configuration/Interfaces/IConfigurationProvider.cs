using Microsoft.Extensions.Logging;

namespace Avvo.Core.Configuration.Interfaces;

/// <summary>
/// Define um provedor de configuração.
/// </summary>
public interface IConfigurationProvider
{
    /// <summary>
    /// Carrega configurações do provedor.
    /// </summary>
    /// <param name="logger">O logger para registro de eventos.</param>
    /// <returns>Uma lista com os parâmetros carregados.</returns>
    Task<List<ConfigurationProviderParameter>> LoadAsync(ILogger? logger);
}
