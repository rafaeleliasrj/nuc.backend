using System.Text.RegularExpressions;
using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging.Aws
{
    /// <summary>
    /// This class is a helper class for working with Aws Resource names.
    /// </summary>
    public class AwsResourceNameHelper
    {
        /// <summary>
        /// This method is called to create an AWS resource safe name.
        /// </summary>
        /// <param name="name">The name to convert</param>
        /// <param name="isFifo">Determines if the queue is the fifo type</param>
        public string CreateSafeName(string name, bool isFifo = false)
        {
            string safeName = Regex.Replace($"{Namespace.Prefix}{name}", @"[^a-zA-Z\d_\-\.]", string.Empty);
            safeName = safeName.Trim();

            if (isFifo)
            {
                if (safeName.Length > 70)
                {
                    safeName = safeName.Substring(0, 70);
                }

                if (safeName.Substring(safeName.Length - 5) != ".fifo")
                {
                    safeName += ".fifo";
                }
            }
            else
            {
                if (safeName.Length > 75)
                {
                    safeName = safeName.Substring(0, 75);
                }
            }

            return safeName;
        }

        /// <summary>
        /// This method is called to create an AWS SNS safe name.
        /// </summary>
        /// <param name="topic">The topic to create the name for.</param>
        public string CreateSafeName(ITopic topic)
        {
            return this.CreateSafeName(topic.Name);
        }

        /// <summary>
        /// This method is called to create an AWS SQS safe name.
        /// </summary>
        /// <param name="queue">The queue to create the name for.</param>
        public string CreateSafeName(IQueue queue)
        {
            return this.CreateSafeName(queue.Name);
        }
    }
}
