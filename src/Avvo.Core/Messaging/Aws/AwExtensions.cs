using Microsoft.Extensions.DependencyInjection;
using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging.Aws
{
    /// <summary>
    /// This class provides AWS related extension methods
    /// </summary>
    public static class AwExtensions
    {
        /// <summary>
        /// This method is called to configure the dependencies required for Aws Messaging
        /// </summary>
        /// <param name="serviceCollection">The IServiceCollection to use</param>
        public static void ConfigureAwsMessaging(this IServiceCollection serviceCollection)
        {
            IAwsQueueService queueService = new AwsQueueService();
            serviceCollection.AddSingleton<IQueueService>(sp => queueService);
            serviceCollection.AddSingleton<IAwsQueueService>(sp => queueService);

            IAwsTopicService topicService = new AwsTopicService();
            serviceCollection.AddSingleton<ITopicService>(sp => topicService);
            serviceCollection.AddSingleton<IAwsTopicService>(sp => topicService);

            serviceCollection.AddSingleton<ISubscriber, AwsSubscriber>();
            serviceCollection.AddSingleton<IQueueWorker, AwsQueueWorker>();
            serviceCollection.AddSingleton<ITopicPublisher, AwsPublisher>();
        }
    }
}
