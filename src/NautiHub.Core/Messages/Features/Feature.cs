using MediatR;

namespace NautiHub.Core.Messages.Features;

public abstract class Feature<TResponse> : IRequest<TResponse>
{
    protected DateTime Timestamp { get; private set; }

    protected Feature()
    {
        Timestamp = DateTime.UtcNow;
    }
}
