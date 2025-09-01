using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging
{
    public class Queue : IQueue
    {
        /// <summary>
        /// This is the name of this queue.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This is the retry policy for this queue.
        /// </summary>
        public IRetryPolicy RetryPolicy { get; set; }

        /// <summary>
        /// This is used to specify if this queue has a Dlq or not
        /// </summary>
        public bool EnableDlq { get; set; }

        /// <summary>
        /// This is used to specify if this queue is the type fifo or not
        /// </summary>

        public bool FifoQueue { get; set; } = false;

        /// <summary>
        /// This is the default constructor.
        /// </summary>
        public Queue()
        {
            this.RetryPolicy = new RetryPolicy();
        }
    }
}
