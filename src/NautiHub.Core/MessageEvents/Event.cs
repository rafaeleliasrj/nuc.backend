using MediatR;

namespace NautiHub.Core.MessageEvents;

public abstract class Event : INotification
{
    protected Event()
    {
        Timestamp = DateTime.UtcNow;
        Name = GetType().Name;
    }

    public DateTime Timestamp { get; private set; }

    public string Name { get; private set; }
}
