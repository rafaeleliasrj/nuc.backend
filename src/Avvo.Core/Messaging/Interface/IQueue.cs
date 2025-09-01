namespace Avvo.Core.Messaging.Interface
{
    /// <summary>
    /// This interface defines a message queue.
    /// </summary>
    public interface IQueue
    {
        /// <summary>
        /// This is the name of this queue.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// This is the retry policy for this queue.
        /// </summary>
        IRetryPolicy RetryPolicy { get; }

        /// <summary>
        /// This is used to specify if this queue has a Dlq or not
        /// </summary>
        bool EnableDlq { get; }

        /// <summary>
        /// This is used to specify if this queue is the type fifo or not
        /// </summary>
        bool FifoQueue { get; }
    }
}
