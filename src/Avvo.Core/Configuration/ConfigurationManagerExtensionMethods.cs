using Amazon.SimpleSystemsManagement;
using Avvo.Core.Commons.Utils;
using Avvo.Core.Configuration.Interfaces;
using Avvo.Core.Logging;

namespace Avvo.Core.Configuration;

/// <summary>
/// Métodos de extensão para o <see cref="IConfigurationManager"/>.
/// </summary>
public static class ConfigurationManagerExtensionMethods
{
    /// <summary>
    /// Adiciona o provedor de configuração AWS ao gerenciador.
    /// </summary>
    /// <param name="configurationManager">O gerenciador de configuração.</param>
    /// <param name="applicationDetails">Os detalhes da aplicação.</param>
    /// <param name="parameters">A lista de parâmetros a solicitar.</param>
    /// <param name="region">A região AWS a ser usada (padrão: "us-east-1").</param>
    /// <exception cref="ArgumentNullException">Lançada se configurationManager, applicationDetails ou parameters forem nulos.</exception>
    public static void AddAwsProvider(this IConfigurationManager configurationManager, IApplicationDetails applicationDetails, IReadOnlyList<ConfigurationProviderParameter> parameters, string region = "us-east-1")
    {
        if (configurationManager == null)
            throw new ArgumentNullException(nameof(configurationManager));
        if (applicationDetails == null)
            throw new ArgumentNullException(nameof(applicationDetails));
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));

        var config = new AmazonSimpleSystemsManagementConfig();
        var awsRegion = EnvironmentVariables.GetOrDefault("AWS_DEFAULT_REGION", EnvironmentVariables.GetOrDefault("AWS_REGION", region));
        config.RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsRegion);

        var serviceUrl = EnvironmentVariables.GetOrDefault("AWS_SSM_URL", null);
        if (serviceUrl != null)
        {
            config.ServiceURL = serviceUrl;
        }

        AmazonSimpleSystemsManagementClient client;
        var accessKey = EnvironmentVariables.Get("AWS_ACCESS_KEY_ID");
        if (accessKey != null)
        {
            var secretAccessKey = EnvironmentVariables.GetOrDefault("AWS_SECRET_ACCESS_KEY", string.Empty);
            client = new AmazonSimpleSystemsManagementClient(accessKey, secretAccessKey, config);
        }
        else
        {
            client = new AmazonSimpleSystemsManagementClient(config);
        }

        var provider = new AwsConfigurationProvider(client, applicationDetails, parameters);
        configurationManager.AddProvider(provider);
    }
}
