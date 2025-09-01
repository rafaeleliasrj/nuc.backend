using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging.Exceptions
{
    public class UnknownQueueException : System.Exception
    {
        /// <summary>
        /// This is the default constructor
        /// </summary>
        public UnknownQueueException() : base($"Unknown queue.") { }

        /// <summary>
        /// This constructor is used to specify a queue
        /// </summary>
        /// <param name="queue">The IQueue that is unknown</param>
        public UnknownQueueException(IQueue queue) : base($"Unknown queue: {queue.Name}") { }
    }
}
