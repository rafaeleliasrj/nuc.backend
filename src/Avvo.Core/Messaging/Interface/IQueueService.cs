using System.Threading.Tasks;

namespace Avvo.Core.Messaging.Interface
{
    /// <summary>
    /// This interface defines a queue service.
    /// </summary>
    public interface IQueueService
    {
        /// <summary>
        /// This method is called to create a queue.
        /// </summary>
        /// <param name="queue">The queue to create</param>
        Task CreateQueueAsync(IQueue queue);

        /// <summary>
        /// This method is called to create a queue.
        /// </summary>
        /// <typeparam name="T">The type of the IQueue</typeparam>
        Task CreateQueueAsync<T>()
        where T : IQueue;

        /// <summary>
        /// This method is called to send a message to a queue
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="queue">The queue to publish the message to</param>
        /// <param name="fifoMessageGroupId">The MessageGroupId of the message, only for FIFO Queues</param>
        Task PublishAsync(object message, IQueue queue, string fifoMessageGroupId = null);
    }
}
