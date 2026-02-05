using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using NautiHub.Core.Aws;
using Newtonsoft.Json;

namespace NautiHub.Core.MessageEvents;

public class EventConsumer(
    ILogger<IEventConsumer> logger,
    IClientAws apiClientAws,
    IAmazonSQS clientAws
) : IEventConsumer
{
    private readonly ILogger<IEventConsumer> _logger = logger;
    private readonly IAmazonSQS _clientAws = clientAws;
    private readonly string _nameEventGroup = apiClientAws.GetEventGroup();

    private const int DELAY_CONSULTA_EVENTOS_SEGUNDOS = 2;
    private const int QUANTIDADE_RETENTATIVAS = 3;
    private const int DELAY_RETENTATIVA_SEGUNDOS = 3;

    private Dictionary<string, string> _bufferFilas = [];

    public async Task AddConsumer<TEventBasic>(
        IEventHandler<TEventBasic> eventHandler,
        int simultaneousExecutions = 5
    )
        where TEventBasic : Event
    {
        _logger.LogInformation(
            "Iniciando monitoramento do evento {eventHandler}.",
            ReturnsFullEventName(eventHandler)
        );

        var request = new ReceiveMessageRequest()
        {
            WaitTimeSeconds = 20,
            QueueUrl = await GetQueueName(ReturnsFullEventName(eventHandler)),
            MaxNumberOfMessages = simultaneousExecutions,
            VisibilityTimeout = 600
        };

        while (true)
        {
            try
            {
                await ProcessEvent(eventHandler, request, simultaneousExecutions);
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Evento não executado devido a exceção não tratada. '{Name}'.Detalhe: {Message}.",
                    ReturnsFullEventName(eventHandler),
                    ex.Message
                );

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(DELAY_CONSULTA_EVENTOS_SEGUNDOS));
                }
                catch { }
            }
        }
    }

    private string ReturnsFullEventName<TEventBasic>(
        IEventHandler<TEventBasic> eventHandler
    )
        where TEventBasic : Event
    {
        var eventName = typeof(TEventBasic).Name;
        if (!string.IsNullOrEmpty(_nameEventGroup))
        {
            eventName = $"{_nameEventGroup}-{eventName}";
        }

        return eventName;
    }

    private async Task ProcessEvent<TEventBasic>(
        IEventHandler<TEventBasic> eventHandler,
        ReceiveMessageRequest request,
        int simultaneousExecutions
    )
        where TEventBasic : Event
    {
        ReceiveMessageResponse response = await _clientAws.ReceiveMessageAsync(request);

        var messageCount = response?.Messages?.Count ?? 0;

        if (messageCount == 0)
            return;

        _logger.LogInformation(
            "Lista mensagens obtidas para o evento '{Name}'. Quantidade de mensagens: {quantidadeMensagens}.",
            ReturnsFullEventName(eventHandler),
            messageCount
        );

        await Parallel.ForEachAsync(
            response!.Messages,
            async (message, cancellationToken) =>
            {
                _logger.LogInformation(
                    "Evento '{Name}'. Mensagem encaminhada para processamento: {MessageId}.",
                    ReturnsFullEventName(eventHandler),
                    message.MessageId
                );

                await this.ConsumeQueue(eventHandler, request, message);
            }
        );

        _logger.LogInformation(
            "Evento '{Name}'. Mensagens processadas com sucesso.",
            ReturnsFullEventName(eventHandler)
        );
    }

    private async Task ConsumeQueue<TEventBasic>(
        IEventHandler<TEventBasic> eventHandler,
        ReceiveMessageRequest request,
        Message message
    )
        where TEventBasic : Event
    {
        bool deleteMessage = false;
        var attemptNumber = 0;

        while (true)
        {
            try
            {
                _logger.LogInformation(
                    "Iniciando processamento do evento '{Name}'. MessageId: {MessageId}.",
                    ReturnsFullEventName(eventHandler),
                    message.MessageId
                );

                TEventBasic? deserializedEvent = JsonConvert.DeserializeObject<TEventBasic>(
                    message.Body
                );

                if (deserializedEvent == null)
                    return;

                await eventHandler.OnExecuteConsume(deserializedEvent);

                _logger.LogInformation(
                    "Evento '{Name}' executado com sucesso. MessageId: {MessageId}.",
                    ReturnsFullEventName(eventHandler),
                    message.MessageId
                );

                deleteMessage = true;

                break;
            }
            catch (Exception ex)
            {
                attemptNumber++;

                _logger.LogError(
                        ex,
                        "Evento não executado '{Name}'. MessageId: {MessageId}, Detalhe: {Message}.",
                        ReturnsFullEventName(eventHandler),
                        message.MessageId,
                        ex.Message
                    );


                if (attemptNumber >= QUANTIDADE_RETENTATIVAS)
                {
                    deleteMessage = true;
                    break;
                }

                _logger.LogWarning(
                    ex,
                    "Erro ao processar o evento '{Name}'. Tentativa: {numeroDeTentativas}, messageId: {MessageId}, detalhe: {Message}.",
                    ReturnsFullEventName(eventHandler),
                    attemptNumber,
                    message.MessageId,
                    ex.Message
                );

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(DELAY_RETENTATIVA_SEGUNDOS));
                }
                catch { }

            }
        }

        if (deleteMessage)
        {
            try
            {
                await _clientAws.DeleteMessageAsync(request.QueueUrl, message.ReceiptHandle);
            }
            catch (Exception)
            {
                _logger.LogError(
                    "Evento '{Name}' executado com sucesso porem não foi possível excluir a mensagem da fila. MessageId: {MessageId}.",
                    ReturnsFullEventName(eventHandler),
                    message.MessageId
                );
            }
        }
        else
        {

        }
    }

    private async Task<string> GetQueueName(string eventName)
    {
        var queueName = _bufferFilas.GetValueOrDefault(eventName) ?? "";

        if (!string.IsNullOrEmpty(queueName))
            return queueName;

        ListQueuesResponse exist = await _clientAws.ListQueuesAsync(eventName);
        queueName = exist.QueueUrls?.FirstOrDefault() ?? null;

        if (string.IsNullOrEmpty(queueName))
        {
            CreateQueueResponse responseCreate = await _clientAws.CreateQueueAsync(
                new CreateQueueRequest { QueueName = eventName }
            );
            queueName = responseCreate.QueueUrl;
        }

        _bufferFilas.Add(eventName, queueName);

        return queueName;
    }
}
