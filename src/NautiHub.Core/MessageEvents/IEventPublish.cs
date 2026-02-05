namespace NautiHub.Core.MessageEvents;

public interface IEventPublish
{
    public Task Publish(Event eventPublish);
}
