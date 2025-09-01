using System.Collections.Concurrent;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using Avvo.Core.Messaging.Exceptions;
using Avvo.Core.Messaging.Interface;
using Avvo.Core.Logging.Correlation;

namespace Avvo.Core.Messaging.Aws
{
    /// <summary>
    /// Implementa os métodos necessários para gerenciar filas AWS. É possível
    /// criar filas, enviar e receber mensagens.
    /// </summary>
    public class AwsQueueService : IAwsQueueService
    {
        /// <summary>
        /// This is the resource name helper.
        /// </summary>
        private readonly AwsResourceNameHelper _nameHelper;

        /// <summary>
        /// This is the queue arn store for created queue.
        /// </summary>
        private readonly ConcurrentDictionary<string, string> _queueArns;

        /// <summary>
        /// This is the queue url store for created queues.
        /// </summary>
        private readonly ConcurrentDictionary<string, string> _queueUrls;

        /// <summary>
        /// This is the Client to use for Aws SQS interactions.
        /// </summary>
        public AmazonSQSClient Client { get; }

        /// <summary>
        /// This is the visibility timeout to use for created queues.
        /// Default is 60 seconds when not specified.
        /// </summary>
        public static string VisibilityTimeout
        {
            get
            {
                return Environment.GetEnvironmentVariable("AWS_VISIBILITY_TIMEOUT") ?? "60";
            }
        }

        /// <summary>
        /// This is the message retention period to use for created queues.
        /// Default is 14 days when not specified.
        /// </summary>
        public static string MessageRetentionPeriod
        {
            get
            {
                return Environment.GetEnvironmentVariable("AWS_MESSAGE_RETENTION_PERIOD") ?? "1209600";
            }
        }

        /// <summary>
        /// Inicializa uma nova instância de AwsQueueService, cria um novo
        /// AmazonSQSClient com as credenciais e configuração de endpoint
        /// obtidas das variáveis de ambiente.
        /// </summary>
        public AwsQueueService()
        {
            AmazonSQSConfig config = new AmazonSQSConfig();
            if (Environment.GetEnvironmentVariable("AWS_SQS_ENDPOINT") != null)
            {
                config.ServiceURL = Environment.GetEnvironmentVariable("AWS_SQS_ENDPOINT");
            }

            AWSCredentials? credentials = null;
            if (Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") != null)
            {
                // take credentials from environment variables
                credentials = new EnvironmentVariablesAWSCredentials();
                Client = new AmazonSQSClient(credentials, config);
            }
            else
            {
                // take credentials from instance profile
                Client = new AmazonSQSClient(config);
            }

            _nameHelper = new AwsResourceNameHelper();
            _queueArns = new ConcurrentDictionary<string, string>(4, 10);
            _queueUrls = new ConcurrentDictionary<string, string>(4, 10);
        }

        /// <summary>
        /// Cria uma fila SQS.
        /// </summary>
        /// <typeparam name="T">The type of the IQueue</typeparam>
        public async Task CreateQueueAsync<T>()
        where T : IQueue
        {
            IQueue queue = Activator.CreateInstance<T>();
            await CreateQueueAsync(queue).ConfigureAwait(false);
        }

        /// <summary>
        /// Cria uma queue SQS e caso necessário sua Dead Letter Queue.
        /// </summary>
        /// <param name="queue">The queue to create.</param>
        public async Task CreateQueueAsync(IQueue queue)
        {
            // create the sqs safe name for the queue
            string queueName = _nameHelper.CreateSafeName(queue.Name, queue.FifoQueue);

            // create the sqs queue
            CreateQueueRequest request = new CreateQueueRequest(queueName);
            request.Attributes.Add("VisibilityTimeout", VisibilityTimeout);
            request.Attributes.Add("MessageRetentionPeriod", MessageRetentionPeriod);

            if (queue.FifoQueue)
            {
                request.Attributes.Add("FifoQueue", "true");
                request.Attributes.Add("ContentBasedDeduplication", "true");
            }

            var response = await Client.CreateQueueAsync(request).ConfigureAwait(false);

            if (!_queueUrls.ContainsKey(queue.Name) && !_queueUrls.TryAdd(queue.Name, response.QueueUrl))
            {
                throw new InvalidOperationException("Unable to add QueueUrl to collection.");
            }

            if (!_queueArns.ContainsKey(queue.Name))
            {
                // fetch the queue arn
                var attributeResponse = await Client.GetQueueAttributesAsync(
                    response.QueueUrl, new List<string>() { "QueueArn" }).ConfigureAwait(false);

                // store the queue arn
                if (!_queueArns.TryAdd(queue.Name, attributeResponse.Attributes["QueueArn"]))
                {
                    throw new InvalidOperationException("Unable to add QueueArn to collection.");
                }
            }

            if (queue.EnableDlq)
            {
                string dlqName = DlqName(queue);
                await CreateQueueAsync(new Queue { Name = dlqName }).ConfigureAwait(false);

                var redrivePolicy = new
                {
                    deadLetterTargetArn = _queueArns[dlqName],
                    maxReceiveCount = queue.RetryPolicy.MaxRetries.ToString()
                };

                var setQueueAttributeRequest = new SetQueueAttributesRequest
                {
                    Attributes = new Dictionary<string, string> { { "RedrivePolicy", JsonConvert.SerializeObject(redrivePolicy) } },
                    QueueUrl = _queueUrls[queue.Name]
                };

                await Client.SetQueueAttributesAsync(setQueueAttributeRequest).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Envia uma mensagem para a dead letter queue.
        /// </summary>
        /// <param name="message">The IMessage to send</param>
        /// <param name="queue">The source Queue to send to the Dlq of</param>
        public async Task SendToDlqAsync(IMessage message, IQueue queue)
        {
            string json = JsonConvert.SerializeObject(new { Message = JsonConvert.SerializeObject(message) });
            await Client.SendMessageAsync(_queueUrls[DlqName(queue)], json).ConfigureAwait(false);
        }

        /// <summary>
        /// Enfileira uma mensagem na queue.
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="queue">The queue to publish the message to</param>
        /// <param name="fifoMessageGroupId">The MessageGroupId of the message, only for FIFO Queues</param>
        public async Task PublishAsync(object message, IQueue queue, string? fifoMessageGroupId = null)
        {
            var correlationService = new CorrelationService();
            IMessage payload = new Message
            {
                Content = message,
                Correlation = correlationService.CreateRequestHeader()
            };

            string json = JsonConvert.SerializeObject(new { Message = JsonConvert.SerializeObject(payload) });
            if (queue.FifoQueue)
            {
                await Client.SendMessageAsync(new SendMessageRequest
                {
                    QueueUrl = _queueUrls[queue.Name],
                    MessageBody = json,
                    MessageGroupId = fifoMessageGroupId
                }).ConfigureAwait(false);
            }
            else
            {
                await Client.SendMessageAsync(_queueUrls[queue.Name], json).ConfigureAwait(false);
            }

        }

        /// <summary>
        /// Recupera o Amazon Resource Name (ARN) da queue.
        /// </summary>
        /// <param name="queue">The queue to get the arn for.</param>
        public string GetQueueArn(IQueue queue)
        {
            // check if the queue arn exists
            if (!_queueArns.ContainsKey(queue.Name))
            {
                throw new UnknownQueueException(queue);
            }

            return _queueArns[queue.Name];
        }

        /// <summary>
        /// Recupera o Universal Resource Locator (URL) da queue.
        /// </summary>
        /// <param name="queue">The queue to get the url for.</param>
        /// <returns>string</returns>
        public string GetQueueUrl(IQueue queue)
        {
            // check if the queue url exists
            if (!_queueUrls.ContainsKey(queue.Name))
            {
                throw new UnknownQueueException(queue);
            }

            return _queueUrls[queue.Name];
        }

        /// <summary>
        /// Recebe uma mensagem da fila.
        /// </summary>
        /// <param name="queue">The queue to receive from.</param>
        /// <param name="waitTime">The time to block and wait for a message.</param>
        public async Task<AwsMessageResponse?> ReceiveMessageAsync(IQueue queue, int waitTime = 0)
        {
            string queueUrl = GetQueueUrl(queue);

            var request = new ReceiveMessageRequest
            {
                MessageSystemAttributeNames = new List<string> { "ApproximateReceiveCount" },
                MaxNumberOfMessages = 1,
                QueueUrl = queueUrl,
                WaitTimeSeconds = waitTime
            };

            var response = await Client.ReceiveMessageAsync(request).ConfigureAwait(false);

            if (response.Messages.Count == 0)
            {
                return null;
            }

            response.Messages[0].Attributes.TryGetValue("ApproximateReceiveCount", out string? retry_attempts_raw);
            int retry_attempts = 0;
            if (retry_attempts_raw != null)
            {
                retry_attempts = int.Parse(retry_attempts_raw) - 1;
            }

            string jsonBody = response.Messages[0].Body;
            AwsMessageBody? body = null;
            Message? message = new Message();

            try
            {
                body = JsonConvert.DeserializeObject<AwsMessageBody>(jsonBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Conteúdo inesperado ao deserializar mensagem AwsMessageBody: {jsonBody} - {ex}");
                if (!string.IsNullOrEmpty(jsonBody))
                    message!.Content = JsonConvert.DeserializeObject(jsonBody)!;
            }

            try
            {
                if (body != null)
                    message = JsonConvert.DeserializeObject<Message>(body.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Conteúdo inesperado ao deserializar mensagem AwsMessageBody: {jsonBody} - Avvo Message: {body?.Message} - {ex}");
                message!.Content = JsonConvert.DeserializeObject(jsonBody)!;
            }

            return new AwsMessageResponse
            {
                Message = message!,
                Args = new MessageArgs
                {
                    RetryAttempts = retry_attempts,
                    Raw = body!.Message,
                    ReceiptHandle = response.Messages[0].ReceiptHandle
                }
            };
        }

        /// <summary>
        /// Remove uma mensagem da fila.
        /// </summary>
        /// <param name="queue">The IQueue the message belongs to.</param>
        /// <param name="receiptHandle">The unique receipt handle of the message.</param>
        public async Task DeleteMessageAsync(IQueue queue, string receiptHandle)
        {
            string queueUrl = GetQueueUrl(queue);
            await Client.DeleteMessageAsync(queueUrl, receiptHandle).ConfigureAwait(false);
        }

        /// <summary>
        /// Altera o tempo de timeout de visibilidade de uma mensagem.
        /// Logo após o recebimento de uma mensagem, ela permanece na fila. Para evitar que
        /// outros consumidores processem a mensagem novamente, o Amazon SQS define um tempo
        /// limite de visibilidade, um período durante o qual o Amazon SQS impede que outros
        /// consumidores recebam e processem a mensagem. O tempo limite de visibilidade
        /// padrão para uma mensagem é de 30 segundos. O mínimo é de 0 segundos. O máximo,
        /// 12 horas.
        /// </summary>
        /// <param name="queue">The IQueue the message belongs to.</param>
        /// <param name="receiptHandle">The unique receipt handle of the message.</param>
        /// <param name="timeout">The visibility timeout in seconds</param>
        public async Task ChangeMessageVisibilityAsync(IQueue queue, string receiptHandle, int timeout)
        {
            string queueUrl = GetQueueUrl(queue);
            await Client.ChangeMessageVisibilityAsync(queueUrl, receiptHandle, timeout).ConfigureAwait(false);
        }

        /// <summary>
        /// This method is called to get the Dlq name for a queue.
        /// </summary>
        /// <param name="queue">The IQueue to get the Dlq name for</param>
        private static string DlqName(IQueue queue)
        {
            return $"{queue.Name}_dlq";
        }
    }
}
