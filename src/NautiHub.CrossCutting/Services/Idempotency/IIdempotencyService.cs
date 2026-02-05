namespace NautiHub.CrossCutting.Services.Idempotency;

public interface IIdempotencyService
{
    public Task<bool> IsAlreadyProcessedAsync(Guid requestId);
    public Task MarkAsProcessedAsync(Guid requestId);
}
