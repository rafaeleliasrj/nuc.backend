using System.Threading.Tasks;

namespace Avvo.Core.Messaging.Interface
{
    /// <summary>
    /// This interface defines a message handler
    /// </summary>
    public interface IMessageHandler
    {
        /// <summary>
        /// This method is called to Process a received queue message.
        /// </summary>
        /// <param name="message">The queue message to process.</param>
        /// <param name="args">The related args for the received queue message.</param>
        Task ProcessMessage(IMessage message, IMessageArgs args);

        /// <summary>
        /// This method is called to determine if this handler can process a received message.
        /// </summary>
        /// <param name="message">The queue message to process.</param>
        /// <param name="args">The related args for the received queue message.</param>
        bool CanProcess(IMessage message, IMessageArgs args);
    }
}
