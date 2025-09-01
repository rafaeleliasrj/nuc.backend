namespace Avvo.Core.Messaging.Interface
{
    /// <summary>
    /// This interface defines a middleware component to process received messages prior to handler processing
    /// </summary>
    public interface IMessageMiddleware
    {
        /// <summary>
        /// This method is called to invoke this middleware
        /// </summary>
        /// <param name="message">The received message</param>
        /// <param name="args">The received message args</param>
        void Invoke(IMessage message, IMessageArgs args);
    }
}
