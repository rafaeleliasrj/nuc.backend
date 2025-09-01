using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging
{
    public class MessageArgs : IMessageArgs
    {
        /// <summary>
        /// This is the number of times this message has been retried.
        /// </summary>
        public int RetryAttempts { get; set; }

        /// <summary>
        /// This specifies if this message should be aborted.
        /// </summary>
        public bool Aborted { get; private set; }

        /// <summary>
        /// This is the raw message contents.
        /// </summary>
        public string Raw { get; set; }

        /// <summary>
        /// This is the receipt handle of this message.
        /// </summary>
        public string ReceiptHandle { get; set; }

        /// <summary>
        /// This indicates if this message has been marked as failed and should be moved to the dlq if enabled
        /// </summary>
        public bool Failed { get; private set; }

        /// <summary>
        /// This indicates if this message has been marked as ignored
        /// </summary>
        public bool Ignored { get; private set; }

        /// <summary>
        /// This method is called to flag this message as aborted.
        /// </summary>
        public void Abort()
        {
            this.Aborted = true;
        }

        /// <summary>
        /// This method is called to flag the message as failed to process, and move to dlq if enabled
        /// </summary>
        public void Fail()
        {
            this.Failed = true;
        }

        /// <summary>
        /// This method is called to flag the message as ignored and not processed.
        /// </summary>
        public void Ignore()
        {
            this.Ignored = true;
        }
    }
}
