using System.Collections.Concurrent;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon;
using Avvo.Core.Messaging.Interface;
using Avvo.Core.Messaging.Exceptions;

namespace Avvo.Core.Messaging.Aws
{
    /// <summary>
    /// This is the Aws Topic Service implementation.
    /// </summary>
    public class AwsTopicService : IAwsTopicService
    {
        /// <summary>
        /// This is the topic arn store for created topics.
        /// </summary>
        private ConcurrentDictionary<string, string> topicArns;

        /// <summary>
        /// This is the safe name store for created topics.
        /// </summary>
        private ConcurrentDictionary<string, string> topicSafeNames;

        /// <summary>
        /// This is the resource name helper.
        /// </summary>
        private AwsResourceNameHelper nameHelper;

        /// <summary>
        /// This is the Client used for Aws SNS interactions.
        /// </summary>
        public AmazonSimpleNotificationServiceClient Client { get; private set; }

        /// <summary>
        /// Inicializa uma nova inst�ncia de AwsTopicService, criando um novo
        /// AmazonSimpleNotificationServiceClient com as configura��es de endpoint
        /// e credenciais recuperadas das vari�veis de ambiente.
        /// </summary>
        /// <param name="region">The aws region to work with</param>
        public AwsTopicService(RegionEndpoint region = null)
        {
            AmazonSimpleNotificationServiceConfig config = new AmazonSimpleNotificationServiceConfig();
            if (Environment.GetEnvironmentVariable("AWS_SNS_ENDPOINT") != null)
            {
                config.ServiceURL = Environment.GetEnvironmentVariable("AWS_SNS_ENDPOINT");
            }

            if (region != null)
            {
                config.RegionEndpoint = region;
            }

            AWSCredentials credentials = null;
            if (Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") != null)
            {
                credentials = new EnvironmentVariablesAWSCredentials();
                this.Client = new AmazonSimpleNotificationServiceClient(credentials, config);
            }
            else
            {
                this.Client = new AmazonSimpleNotificationServiceClient(config);
            }

            this.topicArns = new ConcurrentDictionary<string, string>(4, 100);
            this.topicSafeNames = new ConcurrentDictionary<string, string>(4, 100);
            this.nameHelper = new AwsResourceNameHelper();
        }

        /// <summary>
        /// This method is called to create an SNS topic.
        /// </summary>
        /// <typeparam name="T">The type of the topic to create.</typeparam>
        public async Task CreateTopicAsync<T>()
        where T : ITopic
        {
            ITopic topic = Activator.CreateInstance<T>();
            await this.CreateTopicAsync(topic);
        }

        /// <summary>
        /// This method is called to create an SNS topic.
        /// </summary>
        /// <param name="topic">The topic to create.</param>
        public async Task CreateTopicAsync(ITopic topic)
        {
            // create the sns safe name for the topic
            string topicName = this.nameHelper.CreateSafeName(topic);

            // create the sns topic
            var response = await this.Client.CreateTopicAsync(topicName).ConfigureAwait(false);

            if (!this.topicArns.ContainsKey(topic.Name))
            {
                // store the topic arn
                if (!this.topicArns.TryAdd(topic.Name, response.TopicArn))
                {
                    throw new InvalidOperationException("Unable to add topic arn to collection.");
                }
            }

            if (!this.topicSafeNames.ContainsKey(topic.Name))
            {
                // store the aws safe resource name
                if (!this.topicSafeNames.TryAdd(topic.Name, topicName))
                {
                    throw new InvalidOperationException("Unable to add topic safe name to collection.");
                }
            }
        }

        /// <summary>
        /// This method is called to get the Aws Safe Resource Name of a topic.
        /// </summary>
        /// <param name="topic">The topic to get the name for.</param>
        public string GetSafeName(ITopic topic)
        {
            // check if the topic safe name exists
            if (!this.topicSafeNames.ContainsKey(topic.Name))
            {
                throw new UnknownTopicException(topic);
            }

            return this.topicSafeNames[topic.Name];
        }

        /// <summary>
        ///  This method is called to get the aws arn for a topic.
        /// </summary>
        /// <param name="topic">The topic to get the arn for.</param>
        public string GetTopicArn(ITopic topic)
        {
            // check if the topic arn exists
            if (!this.topicArns.ContainsKey(topic.Name))
            {
                throw new UnknownTopicException(topic);
            }

            return this.topicArns[topic.Name];
        }

        /// <summary>
        /// This method is called to determine if a specific topic has been registered
        /// </summary>
        /// <typeparam name="T">The type of the topic to check.</typeparam>
        public bool IsRegistered<T>()
        where T : ITopic
        {
            var topic = Activator.CreateInstance<T>();
            return this.topicArns.ContainsKey(topic.Name);
        }

        /// <summary>
        /// This method is called to determine if a specific topic has been registered
        /// </summary>
        /// <param name="topic">The topic to check for</param>
        public bool IsRegistered(ITopic topic)
        {
            return this.topicArns.ContainsKey(topic.Name);
        }
    }
}
