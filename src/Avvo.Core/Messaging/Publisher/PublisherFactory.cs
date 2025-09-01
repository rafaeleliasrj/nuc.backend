namespace Avvo.Core.Messaging.Publisher
{
    using Avvo.Core.Messaging.Aws;
    using Avvo.Core.Messaging.Interface;
    using System.Threading.Tasks;
    using Avvo.Core.Logging.Correlation;

    public static class PublisherFactory
    {
        /// <summary>
        /// Cria o Topic e Queue informados, inscreve
        /// a Queue no Topic e retorna um Publisher
        /// capaz de enviar mensagens ao Topico.
        /// </summary>
        /// <param name="correlationService"></param>
        /// <param name="topic">Descritor do Topic a ser criado.</param>
        /// <param name="queue">Descritor da Queue a ser criada.</param>
        /// <returns></returns>
        public static async Task<ITopicPublisher> CreateAsync(ICorrelationService correlationService, ITopic topic, IQueue queue)
        {
            var topicService = new AwsTopicService();
            await topicService.CreateTopicAsync(topic).ConfigureAwait(false);

            var queueService = new AwsQueueService();
            await queueService.CreateQueueAsync(queue).ConfigureAwait(false);

            var subscriber = new AwsSubscriber(topicService, queueService);
            await subscriber.SubscribeAsync(queue, topic).ConfigureAwait(false);

            return new AwsPublisher(topicService, correlationService);
        }
    }
}
