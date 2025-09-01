using System.Threading.Tasks;

namespace Avvo.Core.Messaging.Interface
{
    /// <summary>
    /// This interface defines a topic subscriber.
    /// </summary>
    public interface ISubscriber
    {
        /// <summary>
        /// This method is called to subscribe a queue to a topic.
        /// </summary>
        /// <param name="queue">The recipient queue.</param>
        /// <param name="topic">The topic to subscribe to.</param>
        Task SubscribeAsync(IQueue queue, ITopic topic);
    }
}
