using Amazon.SQS;
using Amazon.SQS.Model;
using Avvo.Core.Logging.Correlation;
using Avvo.Core.Messaging.Aws;

namespace Avvo.Core.Messaging.Publisher
{
    public class SqsPublisher<TMessage> : ISqsPublisher<TMessage>
    {
        private readonly ICorrelationService _correlationService;
        public string QueueName { get; set; }

        public SqsPublisher(ICorrelationService correlationService)
        {
            _correlationService = correlationService;
        }

        public async Task<PublishResponse> PublishAsync(TMessage message)
        {
            var correlation = _correlationService.CreateRequestHeader();

            if (string.IsNullOrEmpty(QueueName))
            {
                throw new ArgumentNullException(nameof(QueueName));
            }

            var queueService = new AwsQueueService();

            var queueUrl = await GetQueueUrl(queueService.Client, QueueName);

            var response = await SendMessage(queueService.Client, queueUrl, Newtonsoft.Json.JsonConvert.SerializeObject(message), new Dictionary<string, MessageAttributeValue>
            {
                ["CorrelationRequest"] = new MessageAttributeValue
                {
                    DataType = "String",
                    StringValue = correlation.Request.ToString()
                }
            });

            return new PublishResponse(correlation.Request, response.MessageId);
        }

        /// <summary>
        /// Sends a message to an SQS queue.
        /// </summary>
        /// <param name="client">An SQS client object used to send the message.</param>
        /// <param name="queueUrl">The URL of the queue to which to send the
        /// message.</param>
        /// <param name="messageBody">A string representing the body of the
        /// message to be sent to the queue.</param>
        /// <param name="messageAttributes">Attributes for the message to be
        /// sent to the queue.</param>
        /// <returns>A SendMessageResponse object that contains information
        /// about the message that was sent.</returns>
        protected static async Task<SendMessageResponse> SendMessage(
            IAmazonSQS client,
            string queueUrl,
            string messageBody,
            Dictionary<string, MessageAttributeValue> messageAttributes)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                DelaySeconds = 10,
                MessageAttributes = messageAttributes,
                MessageBody = messageBody,
                QueueUrl = queueUrl,
            };

            var response = await client.SendMessageAsync(sendMessageRequest);
            Console.WriteLine($"Sent a message with id : {response.MessageId}");

            return response;
        }

        protected async Task<string> GetQueueUrl(AmazonSQSClient client, string queueName)
        {
            try
            {
                var response = await client.GetQueueUrlAsync(queueName);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine($"The URL for {queueName} is: {response.QueueUrl}");

                    return response.QueueUrl;
                }
                else
                {
                    throw new Exception("Error getting queue URL");
                }
            }
            catch (QueueDoesNotExistException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine($"The queue {queueName} was not found.");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
