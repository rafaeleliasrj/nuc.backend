using Microsoft.EntityFrameworkCore;
using Avvo.Core.Host.DependencyGroups;
using Avvo.Core.Commons.Utils;
using Avvo.Core.Data.Interceptors;
using Avvo.Infra.Context;

namespace Avvo.API.DependencyGroups;

/// <summary>
///     My Sql Dependencies
/// </summary>
public class MySqlDependencies : IDependencyGroup
{
    /// <summary>
    ///     Register
    /// </summary>
    /// <param name="logger">logger</param>
    /// <param name="serviceCollection">service collection</param>
    public void Register(ILogger logger, IServiceCollection serviceCollection)
    {
        var mysqlConnection = EnvironmentVariables.Get("MYSQL_CONNECTION");
        var mysqlDatabase = EnvironmentVariables.Get("MYSQL_DATABASE");
        var connection = $"{mysqlConnection};Database={mysqlDatabase};";
        var dbVersion = EnvironmentVariables.Get("MYSQL_VERSION") ?? "11.8.3";

        serviceCollection.AddTransient<RepositoryDbContextSaveChangesInterceptor>();

        serviceCollection.AddDbContext<RepositoryDbContext>((provider, options) =>
            {
                options.UseMySql(connection, new MySqlServerVersion(new Version(dbVersion)),
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure(3).CommandTimeout(30));
                options.AddInterceptors(provider.GetService<RepositoryDbContextSaveChangesInterceptor>());
                options.UseLazyLoadingProxies();
                options.EnableDetailedErrors();

                var environmentsLogs = new List<string> { "LOCAL", "TEST", "DOCKER" };

                if (environmentsLogs.Contains(Environment.GetEnvironmentVariable("ENVIRONMENT").ToUpperInvariant()))
                {
                    options.LogTo(Console.WriteLine, LogLevel.Information);
                    options.EnableSensitiveDataLogging();
                }
            }
        );
    }
}
