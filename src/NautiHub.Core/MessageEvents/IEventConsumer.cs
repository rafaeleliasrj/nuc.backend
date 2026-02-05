namespace NautiHub.Core.MessageEvents;

public interface IEventConsumer
{
    public Task AddConsumer<TEventBasic>(
        IEventHandler<TEventBasic> eventHandler,
        int simultaneousExecutions = 10
    )
        where TEventBasic : Event;
}
