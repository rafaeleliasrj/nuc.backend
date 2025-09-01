using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging.Aws
{
    /// <summary>
    /// This class defines an Aws message response
    /// </summary>
    public class AwsMessageResponse
    {
        /// <summary>
        /// This is the message
        /// </summary>
        public IMessage Message { get; set; }

        /// <summary>
        /// This is the args associtaed with the message
        /// </summary>
        public IMessageArgs Args { get; set; }
    }
}
