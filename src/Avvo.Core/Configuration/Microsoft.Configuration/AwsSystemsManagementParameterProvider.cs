using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Avvo.Core.Logging;

namespace Avvo.Core.Configuration.Microsoft.Configuration
{
    public class AwsSystemsManagementParameterSource : IConfigurationSource
    {
        private readonly ApplicationDetails _applicationDetails;
        public readonly string _region;
        public readonly ILogger _logger;
        public readonly IEnumerable<ConfigurationProviderParameter> _providers;

        public AwsSystemsManagementParameterSource(ApplicationDetails applicationDetails, string region, ILogger logger, IEnumerable<ConfigurationProviderParameter> providers)
        {
            _applicationDetails = applicationDetails;
            _region = region;
            _logger = logger;
            _providers = providers;
        }

        public global::Microsoft.Extensions.Configuration.IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new AwsSystemsManagementParameterProvider(_applicationDetails, _region, _logger, _providers);
    }

    public class AwsSystemsManagementParameterProvider : ConfigurationProvider
    {
        private readonly ApplicationDetails _applicationDetails;
        public readonly string _region;
        public readonly ILogger _logger;
        public readonly IEnumerable<ConfigurationProviderParameter> _providers;

        public AwsSystemsManagementParameterProvider(ApplicationDetails applicationDetails, string region, ILogger logger, IEnumerable<ConfigurationProviderParameter> providers)
        {
            _applicationDetails = applicationDetails;
            _region = region;
            _logger = logger;
            _providers = providers;
        }

        public override void Load()
        {
            var configurationManager = new ConfigurationManager(_logger);
            configurationManager.AddAwsProvider(_applicationDetails, _providers.ToList(), _region);
            var parameters = configurationManager.GetValues();
            foreach (var param in parameters)
                Data.Add(param.Key, param.Value);
        }
    }
}
