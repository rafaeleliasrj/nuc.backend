using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using NautiHub.Core.Aws;
using Newtonsoft.Json;

namespace NautiHub.Core.MessageEvents;

public class EventPublish(
    ILogger<IEventPublish> logger,
    IClientAws apiClientAws,
    IAmazonSQS clientAws
) : IEventPublish
{
    private readonly ILogger<IEventPublish> _logger = logger;
    private readonly IAmazonSQS _clientAws = clientAws;
    private readonly string _eventGroupName = apiClientAws.GetEventGroup();

    private string ReturnsFullEventName(Event eventPublish)
    {
        var eventName = eventPublish.Name;
        if (!string.IsNullOrEmpty(_eventGroupName))
        {
            eventName = $"{_eventGroupName}-{eventName}";
        }

        return eventName;
    }

    public async Task Publish(Event eventPublish)
    {
        try
        {
            _logger.LogInformation(
                "Iniciando publicação do evento {eventPublish}.",
                eventPublish
            );
            ListQueuesResponse exist = await _clientAws.ListQueuesAsync(
                ReturnsFullEventName(eventPublish)
            );
            var queueName = exist.QueueUrls.FirstOrDefault();

            if (string.IsNullOrEmpty(queueName))
            {
                CreateQueueResponse responseCreate = await _clientAws.CreateQueueAsync(
                    new CreateQueueRequest
                    {
                        QueueName = ReturnsFullEventName(eventPublish)
                    }
                );
                queueName = responseCreate.QueueUrl;
            }

            var messageBody = JsonConvert.SerializeObject(eventPublish);
            SendMessageResponse responseSendMsg = await _clientAws.SendMessageAsync(queueName, messageBody);

            _logger.LogInformation(
                "Evento {eventPublish} publicado com sucesso. MessageId: {MessageId}.",
                eventPublish,
                responseSendMsg.MessageId
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar evento {eventPublish}.", eventPublish);
            throw;
        }
    }
}
