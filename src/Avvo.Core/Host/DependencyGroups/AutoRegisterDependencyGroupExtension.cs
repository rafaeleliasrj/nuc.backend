using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Host.DependencyGroups
{
    public static class AutoRegisterDependencyGroupExtension
    {
        public static void RegisterDependencyGroupFromAssemblies(this IServiceCollection serviceCollection, ILogger logger)
        {
            var serviceDependencyType = typeof(IDependencyGroup);
            var serviceDependencies = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => serviceDependencyType.IsAssignableFrom(p) && !p.IsInterface)
                .ToList();

            if (serviceDependencies.Count != 0)
                serviceDependencies.ForEach(type =>
                {
                    var instance = (IDependencyGroup)Activator.CreateInstance(type);
                    instance.Register(logger, serviceCollection);
                });
        }
    }
}
