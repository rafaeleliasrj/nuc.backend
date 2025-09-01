using Amazon.SimpleNotificationService;

namespace Avvo.Core.Messaging.Interface
{

    /// <summary>
    /// This is the definition of an AwsTopicService
    /// </summary>
    public interface IAwsTopicService : ITopicService
    {
        /// <summary>
        /// This method is called to get the Arn of a sns topic.
        /// </summary>
        /// <param name="topic">The topic to get the arn of.</param>
        string GetTopicArn(ITopic topic);

        /// <summary>
        /// This method is called to get the aws sns safe name of a topic.
        /// </summary>
        /// <param name="topic">The topic to get the safe name of.</param>
        string GetSafeName(ITopic topic);

        /// <summary>
        /// This is the aws sns client.
        /// </summary>
        AmazonSimpleNotificationServiceClient Client { get; }
    }
}
