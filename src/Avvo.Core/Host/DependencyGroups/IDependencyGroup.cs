using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Host.DependencyGroups
{
    public interface IDependencyGroup
    {
        void Register(ILogger logger, IServiceCollection serviceCollection);
    }
}
