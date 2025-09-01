using Amazon.Runtime.SharedInterfaces;
using Amazon.SQS;
using Avvo.Core.Messaging.Aws;
using System.Threading.Tasks;

namespace Avvo.Core.Messaging.Interface
{
  /// <summary>
  /// This interface defines an AWS Queue Service.
  /// </summary>
  public interface IAwsQueueService : IQueueService
  {
    /// <summary>
    /// This method is called to get the url of an sqs queue.
    /// </summary>
    /// <param name="queue">The queue to get the url of.</param>
    string GetQueueUrl(IQueue queue);

    /// <summary>
    /// This method is called to get the Arn of an sqs queue.
    /// </summary>
    /// <param name="queue">The queue to get the Arn of.</param>
    string GetQueueArn(IQueue queue);

    /// <summary>
    /// This is the aws sqs client.
    /// </summary>
    AmazonSQSClient Client { get; }

    /// <summary>
    /// This method is called to receive a message from a queue.
    /// </summary>
    /// <param name="queue">The IQueue to receive from</param>
    /// <param name="waitTime">The time to block and wait for a message.</param>
    Task<AwsMessageResponse?> ReceiveMessageAsync(IQueue queue, int waitTime = 0);

    /// <summary>
    /// This method is called to delete a message from a queue.
    /// </summary>
    /// <param name="queue">The IQueue the message was received from.</param>
    /// <param name="receiptHandle">The unique receipt handle of the message to delete.</param>
    Task DeleteMessageAsync(IQueue queue, string receiptHandle);

    /// <summary>
    /// This method is called to change the visibility timeout of a message in a queue.
    /// </summary>
    /// <param name="queue">The IQueue the message belongs to.</param>
    /// <param name="receiptHandle">The unique receipt handle of the message.</param>
    /// <param name="timeout">The visibility timeout in seconds</param>
    Task ChangeMessageVisibilityAsync(IQueue queue, string receiptHandle, int timeout);

    /// <summary>
    /// This method is called to send a recieved message to a queues Dlq
    /// </summary>
    /// <param name="message">The IMessage to send</param>
    /// <param name="queue">The source Queue to send to the Dlq of</param>
    Task SendToDlqAsync(IMessage message, IQueue queue);
  }
}
