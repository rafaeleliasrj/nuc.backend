using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using Avvo.Core.Commons.Utils;
using Avvo.Core.Commons.Extensions;

namespace Avvo.Infra.Context;

public class RepositoryDbContextFactory : IDesignTimeDbContextFactory<RepositoryDbContext>
{
    private const string TESTENVIRONMENT = "test";
    private const string DEFAULTDATABASE = "avvo_db";

    /// <summary>
    ///     Create DB Context.
    /// </summary>
    /// <param name="args">Args.</param>
    /// <returns>DbContext.</returns>
    public RepositoryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RepositoryDbContext>();

        if (EnvironmentVariables.Get("ENVIRONMENT") == TESTENVIRONMENT)
        {
            optionsBuilder.UseInMemoryDatabase(DEFAULTDATABASE);
        }
        else
        {
            var mysqlConnection = EnvironmentVariables.Get("MYSQL_CONNECTION") ??
                                  @"Server=127.0.0.1;Port=3306;Uid=root;Pwd=root";
            var mysqlDatabase = EnvironmentVariables.Get("MYSQL_DATABASE") ?? DEFAULTDATABASE;
            var mysqlVersion = EnvironmentVariables.Get("MYSQL_VERSION") ?? "5.7.30";
            var connectionString = $"{mysqlConnection};Database={mysqlDatabase};";

            optionsBuilder
                .UseMySql(connectionString, new MySqlServerVersion(new Version(mysqlVersion)))
                // .UseLazyLoadingProxies()
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }

        return new RepositoryDbContext(optionsBuilder.Options);
    }
}
