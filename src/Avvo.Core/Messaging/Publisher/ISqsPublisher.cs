using System.Collections.Generic;
using System.Threading.Tasks;
using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging.Publisher
{
    /// <summary>
    /// This interface defines the publusher.
    /// </summary>
    public interface ISqsPublisher<T>
    {
        /// <summary>
        /// This method is called to register the publisher this worker should use.
        /// </summary>
        /// <param name="message">The content of the message.</param>
        Task<PublishResponse> PublishAsync(T message);
    }
}
