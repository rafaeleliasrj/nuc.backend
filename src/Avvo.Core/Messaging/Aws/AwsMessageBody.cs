namespace Avvo.Core.Messaging.Aws
{
    /// <summary>
    /// This class defines the body of an SNS/SQS message body.
    /// </summary>
    public class AwsMessageBody
    {
        /// <summary>
        /// This is the message that was submitted to SNS as a string.
        /// </summary>
        public string Message { get; set; }
    }
}
