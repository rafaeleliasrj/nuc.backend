using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Configuration.Interfaces;
using Avvo.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Configuration;

/// <summary>
/// Provedor de configuração que carrega valores do AWS Systems Manager Parameter Store.
/// </summary>
public class AwsConfigurationProvider : IConfigurationProvider
{
    private readonly IAmazonSimpleSystemsManagement _client;
    private readonly IReadOnlyList<ConfigurationProviderParameter> _parameters;
    private readonly IApplicationDetails _applicationDetails;

    /// <summary>
    /// Obtém o cliente AWS usado por este provedor.
    /// </summary>
    public IAmazonSimpleSystemsManagement Client => _client;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="AwsConfigurationProvider"/>.
    /// </summary>
    /// <param name="client">O cliente do AWS Systems Manager.</param>
    /// <param name="applicationDetails">Os detalhes da aplicação.</param>
    /// <param name="parameters">A lista de parâmetros a serem carregados.</param>
    /// <exception cref="ArgumentNullException">Lançada se client, applicationDetails ou parameters forem nulos.</exception>
    public AwsConfigurationProvider(IAmazonSimpleSystemsManagement client, IApplicationDetails applicationDetails, IReadOnlyList<ConfigurationProviderParameter> parameters)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _applicationDetails = applicationDetails ?? throw new ArgumentNullException(nameof(applicationDetails));
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
    }

    /// <summary>
    /// Carrega configurações do AWS Parameter Store para o ambiente atual.
    /// </summary>
    /// <param name="logger">O logger para registro de eventos.</param>
    /// <returns>Uma lista com os parâmetros carregados.</returns>
    public async Task<List<ConfigurationProviderParameter>> LoadAsync(ILogger? logger)
    {
        try
        {
            var request = CreateRequest();
            logger?.LogInformation("[{Provider}] - Solicitando {ParameterCount} parâmetros.", nameof(AwsConfigurationProvider), request.Names.Count);

            foreach (var param in request.Names)
            {
                logger?.LogInformation("[{Provider}] - Solicitando parâmetro: {Parameter}", nameof(AwsConfigurationProvider), param);
            }

            var result = await _client.GetParametersAsync(request, CancellationToken.None);

            logger?.LogInformation("[{Provider}] - Recebidos {ParameterCount} parâmetros.", nameof(AwsConfigurationProvider), result.Parameters.Count);

            var returnedParameters = new List<ConfigurationProviderParameter>();
            foreach (var param in result.Parameters)
            {
                var parameter = _parameters.FirstOrDefault(p => p.Name == param.Name.Split('.')[param.Name.Split('.').Length - 1]);
                if (parameter == null)
                {
                    logger?.LogWarning("[{Provider}] - Nenhum ConfigurationProviderParameter encontrado para o parâmetro: {Parameter}", nameof(AwsConfigurationProvider), param.Name);
                    continue;
                }

                logger?.LogInformation("[{Provider}] - Parâmetro recebido: {ParameterName}", nameof(AwsConfigurationProvider), parameter.Name);
                parameter.Value = param.Value;
                returnedParameters.Add(parameter);
            }

            return returnedParameters;
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, $"[{nameof(AwsConfigurationProvider)}] - Erro ao carregar parâmetros do AWS Parameter Store.");
            throw new ServiceException("Erro ao carregar configurações do AWS Parameter Store.", ex);
        }
    }

    /// <summary>
    /// Cria o objeto <see cref="GetParametersRequest"/> necessário para solicitar parâmetros do AWS.
    /// </summary>
    /// <returns>O objeto de solicitação.</returns>
    public GetParametersRequest CreateRequest()
    {
        var request = new GetParametersRequest
        {
            Names = ParameterKeys(),
            WithDecryption = true
        };
        return request;
    }

    /// <summary>
    /// Retorna a lista de chaves de parâmetros a serem solicitadas do AWS Parameter Store.
    /// As chaves incluem prefixos com detalhes da aplicação, como Landscape, Environment, Name e Version.
    /// </summary>
    /// <returns>Lista de nomes de parâmetros a solicitar.</returns>
    public List<string> ParameterKeys()
    {
        var output = new List<string>();
        foreach (var parameter in _parameters)
        {
            var key = parameter.Type switch
            {
                ParameterType.Global => $"{_applicationDetails.Landscape}.{_applicationDetails.ConfigEnvironment}.{parameter.Name}",
                ParameterType.Application => $"{_applicationDetails.Landscape}.{_applicationDetails.ConfigEnvironment}.{_applicationDetails.Name}.{_applicationDetails.ConfigVersion}.{parameter.Name}",
                _ => throw new ServiceException($"Tipo de parâmetro inválido: {parameter.Type}")
            };
            output.Add(key);
        }
        return output;
    }
}
