using Hangfire;

using NautiHub.API.Jobs;

namespace NautiHub.API.Configurations;

internal static class JobsConfiguration
{
    public static IServiceCollection AddJobs(this IServiceCollection services)
    {
        //Jobs do hangfire
        //services.AddScoped<IJob, ExempleJob>();

        return services;
    }

    public static IApplicationBuilder RegistrarJobs(
        this IApplicationBuilder app,
        IServiceProvider serviceProvider
    )
    {
        using (var connection = JobStorage.Current.GetConnection())
        {
            using (var distributedLock = connection.AcquireDistributedLock("job-registration", TimeSpan.FromSeconds(30)))
            {
                foreach (var jobService in serviceProvider.GetServices<IJob>())
                {
                    var nomeDoServico = jobService.GetType().Name;

                    try
                    {
                        RecurringJob.RemoveIfExists(nomeDoServico);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Erro ao remover job para recriação: " + nomeDoServico);
                    }

                    try
                    {
                        RecurringJob.AddOrUpdate(nomeDoServico, () => jobService!.Executar(), jobService.TimerCronJob());
                        Console.WriteLine("Job adicionado: " + nomeDoServico);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Erro ao adicionar job: " + nomeDoServico);
                    }
                }
            }
        }

        return app;
    }
}
