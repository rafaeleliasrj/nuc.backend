using MediatR;

namespace NautiHub.Core.Messages.Queries;

public abstract class Query<TResponse> : IRequest<TResponse>
{
    protected DateTime Timestamp { get; private set; }

    protected Query()
    {
        Timestamp = DateTime.UtcNow;
    }
}
