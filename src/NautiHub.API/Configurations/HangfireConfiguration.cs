using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.PostgreSql;
using System;

namespace NautiHub.API.Configurations;

internal static class HangfireConfiguration
{
    public static IServiceCollection AddHangfireConfiguration(this IServiceCollection services)
    {
        var connectionString = GetConnectionString();

        services.AddHangfire(config =>
            config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(new PostgreSqlStorage(
                    connectionString,
                    new PostgreSqlStorageOptions
                    {
                        SchemaName = "hangfire",
                        QueuePollInterval = TimeSpan.FromSeconds(15),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        PrepareSchemaIfNecessary = true
                    }
                ))
        );

        services.AddHangfireServer(options =>
        {
            options.SchedulePollingInterval = TimeSpan.FromMinutes(1);
            options.HeartbeatInterval = TimeSpan.FromMinutes(1);
            options.ServerCheckInterval = TimeSpan.FromMinutes(3);
            options.Queues = ["default"];
        });

        return services;
    }

    public static IApplicationBuilder AddHangfireDashboard(this IApplicationBuilder app)
    {
        app.UseHangfireDashboardCustomOptions(
            new HangfireDashboardCustomOptions { DashboardTitle = () => "Nauti Hub" }
        );

        var hangfireUser = Environment.GetEnvironmentVariable("HANGFIRE_USER");
        if (string.IsNullOrEmpty(hangfireUser))
        {
            throw new Exception("Variável de ambiente HANGFIRE_USER não informada.");
        }

        var hangfirePassword = Environment.GetEnvironmentVariable("HANGFIRE_PASSWORD");
        if (string.IsNullOrEmpty(hangfirePassword))
        {
            throw new Exception("Variável de ambiente HANGFIRE_PASSWORD não informada.");
        }

        var dashboardOptions = new DashboardOptions
        {
            Authorization =
            [
                new BasicAuthAuthorizationFilter(
                    new BasicAuthAuthorizationFilterOptions
                    {
                        RequireSsl = false,
                        SslRedirect = false,
                        LoginCaseSensitive = true,
                        Users =
                        [
                            new BasicAuthAuthorizationUser
                            {
                                Login = hangfireUser,
                                PasswordClear = hangfirePassword
                            }
                        ]
                    }
                )
            ]
        };

        app.UseHangfireDashboard("/jobs", dashboardOptions);

        return app;
    }

    private static string GetConnectionString()
    {
        var connectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION");
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException();
        return connectionString;
    }
}
