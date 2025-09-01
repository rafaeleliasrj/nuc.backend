using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

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

            serviceDependencies.ForEach(type =>
            {
                logger.LogInformation("Registering service dependency: {0}", type.Name);
                var instance = (IDependencyGroup)Activator.CreateInstance(type);
                instance.Register(logger, serviceCollection);
            });
        }
    }
}
