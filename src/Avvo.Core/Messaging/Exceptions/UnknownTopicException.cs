using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging.Exceptions
{
    public class UnknownTopicException : System.Exception
    {
        /// <summary>
        /// This is the default constructor
        /// </summary>
        /// <param name="topic">The ITopic that is unknown</param>
        public UnknownTopicException(ITopic topic) : base($"Unknown topic: {topic.Name}") { }
    }
}
