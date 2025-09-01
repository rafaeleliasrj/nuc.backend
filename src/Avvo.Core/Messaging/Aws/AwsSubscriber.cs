using System.Threading.Tasks;
using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging.Aws
{
    /// <summary>
    /// Inscreve filas (queues) a um tópico (topic).
    /// Quando uma ou mais filas são inscritas em um tópico,
    /// cada uma recebe notificações idênticas sempre que uma
    /// mensagem é enviada para o tópico. Os serviços anexados
    /// a essas filas podem processar os pedidos de forma
    /// assíncrona e paralela.
    /// </summary>
    public class AwsSubscriber : ISubscriber
    {
        private readonly IAwsTopicService _topicService;
        private readonly IAwsQueueService _queueService;

        /// <summary>
        /// Inicializa uma nova instância de AwsSubscriber.
        /// </summary>
        /// <param name="topicService">The IAwsTopicService to use</param>
        /// <param name="queueService">The IAwsQueueService to use</param>
        public AwsSubscriber(IAwsTopicService topicService, IAwsQueueService queueService)
        {
            _topicService = topicService;
            _queueService = queueService;
        }

        /// <summary>
        /// Inscreve uma fila em um tópico. Mensagens enviadas para o tópico
        /// serão automaticamente publicadas na fila.
        /// </summary>
        /// <param name="queue">The recipient queue.</param>
        /// <param name="topic">The topic to subscribe to.</param>
        public async Task SubscribeAsync(IQueue queue, ITopic topic)
        {
            if (!_topicService.IsRegistered(topic))
            {
                await _topicService.CreateTopicAsync(topic).ConfigureAwait(false);
            }

            string topicArn = _topicService.GetTopicArn(topic);
            string queueUrl = _queueService.GetQueueUrl(queue);
            await _topicService.Client.SubscribeQueueAsync(topicArn, _queueService.Client, queueUrl).ConfigureAwait(false);
        }
    }
}
