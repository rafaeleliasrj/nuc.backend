using Avvo.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Configuration.Microsoft.Configuration
{
    /// <summary>
    ///     <para>
    ///         Todos estes métodos de extensões são para adicionar o comportamente padrão de injeção de configurações da Microsoft de forma compativel com as rotinas já existentes do Avvo.Core
    ///     </para>
    ///     <para>
    ///         Caso deseje fazer a leitura de configuração avulsa sem os padrões do framework, utilize a rotina padrão do AWSSDK <see href="https://aws.amazon.com/pt/blogs/developer/net-core-configuration-provider-for-aws-systems-manager/"/>
    ///     </para>
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAwsSystemsManagementParameters(
            this IConfigurationBuilder builder,
            ILogger logger,
            ApplicationDetails applicationDetails,
            string region,
            params ConfigurationProviderParameter[] providers) => builder.Add(new AwsSystemsManagementParameterSource(applicationDetails, region, logger, providers));

        public static IConfigurationBuilder AddAwsSystemsManagementParameters(
            this IConfigurationBuilder builder,
            ApplicationDetails applicationDetails,
            string region,
            params ConfigurationProviderParameter[] providers) => builder.AddAwsSystemsManagementParameters(null, applicationDetails, region, providers);

        public static IConfigurationBuilder AddAwsSystemsManagementParameters(
            this IConfigurationBuilder builder,
            ILogger logger,
            ApplicationDetails applicationDetails,
            params ConfigurationProviderParameter[] providers) => builder.AddAwsSystemsManagementParameters(logger, applicationDetails, null, providers);

        public static IConfigurationBuilder AddAwsSystemsManagementParameters(
            this IConfigurationBuilder builder,
            ApplicationDetails applicationDetails,
            params ConfigurationProviderParameter[] providers) => builder.AddAwsSystemsManagementParameters(null, applicationDetails, null, providers);
    }
}
