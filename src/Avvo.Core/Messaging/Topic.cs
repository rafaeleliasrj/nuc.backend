using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging
{
    public class Topic : ITopic
    {
        /// <summary>
        /// This is the unique name of this topic.
        /// </summary>
        public string Name { get; set; }
    }
}
