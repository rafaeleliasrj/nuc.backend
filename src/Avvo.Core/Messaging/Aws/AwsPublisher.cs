using System.Text;
using Newtonsoft.Json;
using Avvo.Core.Messaging.Interface;
using Avvo.Core.Logging.Correlation;

namespace Avvo.Core.Messaging.Aws
{
    /// <summary>
    /// Publica mensagens em topicos SNS.
    /// </summary>
    public class AwsPublisher : ITopicPublisher
    {
        private readonly IAwsTopicService _topicService;
        private readonly ICorrelationService _correlationService;

        /// <summary>
        /// Inicializa uma nova instancia de AwsPublisher.
        /// </summary>
        /// <param name="topicService">The IAwsTopicService to use.</param>
        /// <param name="correlationService">The ICorrelationService to use.</param>
        public AwsPublisher(IAwsTopicService topicService, ICorrelationService correlationService)
        {
            _topicService = topicService;
            _correlationService = correlationService;
        }

        /// <summary>
        /// Envia uma mensagem para um topico SNS.
        /// </summary>
        /// <param name="topic">This is the topic to publish the message to.</param>
        /// <param name="message">This is the message to publish.</param>
        /// <param name="identity"></param>
        /// <param name="useDefaultObjectMessage"></param>
        public async Task<PublishResponse> PublishAsync(ITopic topic, object message, Dictionary<string, string> identity, bool useDefaultObjectMessage)
        {
            if (!_topicService.IsRegistered(topic))
            {
                await _topicService.CreateTopicAsync(topic).ConfigureAwait(false);
            }

            string topicArn = _topicService.GetTopicArn(topic);

            string json = string.Empty;

            var correlation = _correlationService.CreateRequestHeader();

            if (useDefaultObjectMessage)
            {
                json = JsonConvert.SerializeObject(message);
            }
            else
            {
                // create the message payload
                Message payload = new Message
                {

                    Content = message,
                    Correlation = correlation,
                    Topic = topic.Name
                };

                if (identity != null)
                {
                    payload.Identity = identity;
                }

                // serialize the message payload into json
                json = JsonConvert.SerializeObject(payload);
            }

            // publish the message payload to the topic
            var result = await _topicService.Client.PublishAsync(topicArn, json).ConfigureAwait(false);

            if (((int)result.HttpStatusCode) >= 200 && ((int)result.HttpStatusCode <= 299))
            {
                return new PublishResponse(correlation.Request, result.MessageId);
            }

            // os erros mais comuns (authorization, internal server error, not found, etc)
            // serao disparados de dentro do PublishAsync, caso acontecam.
            // Na eventualidade de alguma coisa escapar, uma exception com os dados relevantes
            // sera disparada.
            StringBuilder sb = new StringBuilder();
            sb.Append("Error ");
            sb.Append((int)result.HttpStatusCode);
            sb.Append(" while publishing a message. Amazon metadata: ");
            if (result.ResponseMetadata == null)
            {
                sb.Append("[null]");
            }
            else
            {
                sb.Append("Request ID: ");
                sb.Append(result.ResponseMetadata.RequestId);
                if (result.ResponseMetadata.Metadata?.Count > 0)
                {
                    foreach (var current in result.ResponseMetadata.Metadata)
                    {
                        sb.Append(current.Key);
                        sb.Append(": ");
                        sb.Append(current.Value);
                        sb.Append(" ");
                    }
                }
            }

            sb.Append(" Message being published: ");
            sb.Append(json);

            throw new ApplicationException(sb.ToString());
        }
    }
}
