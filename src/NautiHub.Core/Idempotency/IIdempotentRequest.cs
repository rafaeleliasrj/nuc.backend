namespace NautiHub.Core.Idempotency;

public interface IIdempotentRequest
{
    public Guid RequestId { get; }
}
