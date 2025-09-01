namespace Avvo.Core.Messaging.Interface
{
    /// <summary>
    /// This interface defines arguments for a received queue message.
    /// </summary>
    public interface IMessageArgs
    {
        /// <summary>
        /// This is the number of times the message has been retried so far.
        /// </summary>
        int RetryAttempts { get; }

        /// <summary>
        /// This indicates if the message processing has been aborted.
        /// </summary>
        bool Aborted { get; }

        /// <summary>
        /// This method is called to flag the message processing as aborted.
        /// </summary>
        void Abort();

        /// <summary>
        /// This indicates if the message has been marked as failed and should be moved to the dlq if enabled
        /// </summary>
        bool Failed { get; }

        /// <summary>
        /// This method is called to flag the message as failed to process, and move to dlq if enabled
        /// </summary>
        void Fail();

        /// <summary>
        /// This indicates if the message has been marked as ignored and not processed.
        /// </summary>
        bool Ignored { get; }

        /// <summary>
        /// This method is called to flag the message as ignored and not processed
        /// </summary>
        void Ignore();

        /// <summary>
        /// This is the raw message that was received.
        /// </summary>
        string Raw { get; }

        /// <summary>
        /// This is the receipt handle of the message.
        /// </summary>
        string ReceiptHandle { get; }
    }
}
