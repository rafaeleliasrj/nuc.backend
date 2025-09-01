namespace Avvo.Core.Messaging.Interface
{
    /// <summary>
    /// This interface defines a retry policy for a queue.
    /// </summary>
    public interface IRetryPolicy
    {
        /// <summary>
        /// This is the retry delay.
        /// When Backoff = true the retry inteval = Delay * RetryAttempts.
        /// When BackOff = false the retry interval = Delay
        /// </summary>
        int Delay { get; }

        /// <summary>
        /// Should retries apply an incremental backoff.
        /// </summary>
        bool BackOff { get; }

        /// <summary>
        /// The maximum number of times a message should be allowed to be retried.
        /// </summary>
        int MaxRetries { get; }
    }
}
