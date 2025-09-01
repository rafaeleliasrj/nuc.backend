using System.Threading.Tasks;

namespace Avvo.Core.Messaging.Interface
{
    /// <summary>
    /// This interface defines an error handler
    /// </summary>
    public interface IErrorHandler
    {
        /// <summary>
        /// This method is called when an unhandled error occured processing a message.
        /// </summary>
        /// <param name="exception">The unhandled exception that occured.</param>
        /// <param name="message">The queue message that triggered the error.</param>
        /// <param name="args">The related args for the queue message.</param>
        Task OnError(System.Exception exception, IMessage message, IMessageArgs args);
    }
}
