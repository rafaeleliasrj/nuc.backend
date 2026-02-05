namespace NautiHub.API.Jobs;

internal interface IJob
{
    public Task Executar();

    public string TimerCronJob();
}
