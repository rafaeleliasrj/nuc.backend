using MediatR;

namespace NautiHub.Core.Messages.Commands;

public abstract class Command<TResponse> : IRequest<TResponse>
{
    protected DateTime Timestamp { get; private set; }

    protected Command()
    {
        Timestamp = DateTime.UtcNow;
    }
}
