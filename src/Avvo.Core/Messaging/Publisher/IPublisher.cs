using System.Collections.Generic;
using System.Threading.Tasks;
using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging.Publisher
{
    /// <summary>
    /// This interface defines the publusher.
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// This method is called to register the publisher this worker should use.
        /// </summary>
        /// <param name="message">The content of the message.</param>
        /// <param name="queue">Queue name</param>
        /// <param name="topic">Topic name</param>
        /// <param name="identity">The generic identity provider.</param>
        /// <param name="useDefaultObjectMessage">Set this param if you need a default message.</param>
        Task<PublishResponse> PublishAsync(object message, ITopic topic, IQueue queue, Dictionary<string, string> identity = null, bool useDefaultObjectMessage = false);
    }

    /// <summary>
    /// This interface defines the publusher.
    /// </summary>
    public interface IPublisher<T>
    {
        /// <summary>
        /// This method is called to register the publisher this worker should use.
        /// </summary>
        /// <param name="message">The content of the message.</param>
        /// <param name="identity">The generic identity provider.</param>
        /// <param name="useDefaultObjectMessage">Set this param if you need a default message.</param>
        Task<PublishResponse> PublishAsync(T message, Dictionary<string, string> identity = null, bool useDefaultObjectMessage = false);
    }
}
