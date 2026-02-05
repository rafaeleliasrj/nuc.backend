namespace NautiHub.Core.MessageEvents;

public interface IEventHandler<TEventBasic>
    where TEventBasic : Event
{
    public Task OnExecuteConsume(TEventBasic context);
}
