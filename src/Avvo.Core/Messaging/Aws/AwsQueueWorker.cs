using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Avvo.Core.Messaging.Interface;
using Avvo.Core.Messaging.Exceptions;
using Avvo.Core.Logging.Correlation;
using Avvo.Core.Logging;

namespace Avvo.Core.Messaging.Aws
{
    /// <summary>
    /// This class is the Aws Sqs implementation of the IQueueworker interface.
    /// </summary>
    public class AwsQueueWorker : IQueueWorker
    {
        /// <summary>
        /// This is the IQueueService this worker should use.
        /// </summary>
        private readonly IAwsQueueService queueService;

        /// <summary>
        /// This is the ITopicService this worker should use.
        /// </summary>
        private readonly IAwsTopicService topicService;

        /// <summary>
        /// This is the ISubscriber this worker should use.
        /// </summary>
        private readonly ISubscriber subscriber;

        /// <summary>
        /// This is the ICorrelationService this worker should use.
        /// </summary>
        private readonly ICorrelationService correlationService;

        /// <summary>
        /// This is the service collection of the dependency injection framework.
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// This is the logger to use.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The ITraceService to use
        /// </summary>
        private readonly ITraceService traceService;

        /// <summary>
        /// This is the collection of threads this worker is using to process messages.
        /// </summary>
        private List<Thread> threads = new List<Thread>();

        /// <summary>
        /// This specifies if this worker is active or not.
        /// </summary>
        private bool active = false;

        /// <summary>
        /// This is the collection of registered error handlers this worker should use.
        /// </summary>
        private readonly List<IErrorHandler> errorHandlers = new List<IErrorHandler>();

        /// <summary>
        /// This is the collection of registered error handlers.
        /// </summary>
        public List<IErrorHandler> ErrorHandlers
        {
            get { return this.errorHandlers; }
        }

        /// <summary>
        /// This is the collection of registered message handlers this worker should use.
        /// </summary>
        private readonly List<IMessageHandler> messageHandlers = new List<IMessageHandler>();

        /// <summary>
        /// This is the collection of registered message handlers this worker should use.
        /// </summary>
        private readonly List<IMessageMiddleware> middleware = new List<IMessageMiddleware>();

        /// <summary>
        /// This is the collection of registered middleware this worker should use.
        /// </summary>
        public List<IMessageMiddleware> Middleware
        {
            get { return this.middleware; }
        }

        /// <summary>
        /// This is the registered IQueue of this worker.
        /// </summary>
        private IQueue? queue;


        /// <summary>
        /// This is the registered IQueue of this worker.
        /// </summary>
        public IQueue Queue => this.queue ?? throw new InvalidOperationException("Queue not registered");


        /// <summary>
        /// This is the time in seconds to wait on an open connection for a message to be received
        /// </summary>
        public int WaitTime { get; set; }

        /// <summary>
        /// This is the default constructor.
        /// </summary>
        /// <param name="queueService">The IQueueService this queue worker should use.</param>
        /// <param name="topicService">The ITopicService this queue worker should use.</param>
        /// <param name="subscriber">The ISubscriber this queue worker should use.</param>
        /// <param name="correlationService">The ICorrelationService this queue worker should use.</param>
        /// <param name="serviceProvider">The IServiceProvider this queue worker should use.</param>
        /// <param name="logger">The ILogger this queue worker should use.</param>
        /// <param name="traceService">The ITraceService this queue worker should use.</param>
        public AwsQueueWorker(
            IAwsQueueService queueService,
            IAwsTopicService topicService,
            ISubscriber subscriber,
            ICorrelationService correlationService,
            IServiceProvider serviceProvider,
            ILogger logger,
            ITraceService traceService
        )
        {
            this.queueService = queueService;
            this.topicService = topicService;
            this.subscriber = subscriber;
            this.correlationService = correlationService;
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            this.traceService = traceService;
            this.WaitTime = 20;
        }

        /// <summary>
        /// This method is called to register an IErrorHandler with this worker.
        /// </summary>
        /// <typeparam name="T">The IErrorHandler to register.</typeparam>
        public void RegisterErrorHandler<T>()
        where T : IErrorHandler
        {
            if (this.errorHandlers.Find((i) => i.GetType() == typeof(T)) != null)
            {
                // Error handler has already been registered so return
                return;
            }

            IErrorHandler handler = (IErrorHandler)ActivatorUtilities.CreateInstance(this.serviceProvider, typeof(T));
            this.errorHandlers.Add(handler);
        }

        /// <summary>
        /// This method is called to determine if a specific error handler has been registered.
        /// </summary>
        /// <typeparam name="T">The type fo the error handler.</typeparam>
        public bool IsErrorHandlerRegistered<T>()
        where T : IErrorHandler
        {
            return this.errorHandlers.Find(h => h.GetType() == typeof(T)) != null;
        }

        /// <summary>
        /// This method is called to register an IMessageHandler with this worker.
        /// </summary>
        /// <typeparam name="T">The type of the IMessageHandler.</typeparam>
        public void RegisterMessageHandler<T>()
        where T : IMessageHandler
        {
            if (this.messageHandlers.Find((i) => i.GetType() == typeof(T)) != null)
            {
                // Message handler has already been registered so return
                return;
            }

            IMessageHandler handler = (IMessageHandler)ActivatorUtilities.CreateInstance(this.serviceProvider, typeof(T));
            this.messageHandlers.Add(handler);
        }

        /// <summary>
        /// This method is called to determine if a message handler has been registered.
        /// </summary>
        /// <typeparam name="T">The type of the message handler</typeparam>
        public bool IsMessageHandlerRegistered<T>()
        where T : IMessageHandler
        {
            return this.messageHandlers.Find(h => h.GetType() == typeof(T)) != null;
        }

        /// <summary>
        /// This method is called to register a middleware for this worker.
        /// </summary>
        /// <typeparam name="T">The type of the middleware to register</typeparam>
        public void RegisterMiddleware<T>()
        where T : IMessageMiddleware
        {
            if (this.middleware.Find((i) => i.GetType() == typeof(T)) != null)
            {
                // Message handler has already been registered so return
                return;
            }

            IMessageMiddleware mw = (IMessageMiddleware)ActivatorUtilities.CreateInstance(this.serviceProvider, typeof(T));
            this.middleware.Add(mw);
        }

        /// <summary>
        /// This method is called to determine if a middleware has been registered
        /// </summary>
        /// <typeparam name="T">The type of the IMessageMiddleware</typeparam>
        public bool IsMiddlewareRegistered<T>()
        where T : IMessageMiddleware
        {
            return this.middleware.Find(h => h.GetType() == typeof(T)) != null;
        }

        /// <summary>
        /// This method is called to register the queue this worker should use.
        /// </summary>
        /// <param name="queue">The message queue this worker should receive messages from.</param>
        public void RegisterQueue(IQueue queue)
        {
            this.queue = queue;
            this.queueService.CreateQueueAsync(this.queue).Wait();
        }

        /// <summary>
        /// This method is called to register the queue this worker should use.
        /// </summary>
        /// <typeparam name="T">The IQueue type that this worker should receive messages from.</typeparam>
        public void RegisterQueue<T>()
        where T : IQueue
        {
            this.RegisterQueue(Activator.CreateInstance<T>());
        }

        /// <summary>
        /// This method is called to register a topic this worker should subscribe to.
        /// </summary>
        /// <typeparam name="T">This is the ITopic type to register.</typeparam>
        public void RegisterTopic<T>()
        where T : ITopic
        {
            ITopic topic = Activator.CreateInstance<T>();
            this.RegisterTopic(topic);
        }

        /// <summary>
        /// This method is called to register a topic this worker should subscribe to.
        /// </summary>
        /// <param name="topic">The topic to register.</param>
        public void RegisterTopic(ITopic topic)
        {
            this.HasRegisteredQueue();
            this.topicService.CreateTopicAsync(topic).Wait();
            this.subscriber.SubscribeAsync(this.Queue, topic).Wait();
        }

        /// <summary>
        /// This method is called to verify an IQueue has been registered for this worker.
        /// </summary>
        public void HasRegisteredQueue()
        {
            if (this.queue == null)
            {
                throw new UnknownQueueException();
            }
        }

        /// <summary>
        /// This method is called to start this worker.
        /// </summary>
        public void Start()
        {
            this.HasRegisteredQueue();
            this.threads = new List<Thread>();
            int threadCount = this.ThreadCount();
            ThreadPool.SetMinThreads(threadCount, threadCount * 3);
            this.active = true;

            for (int i = 0; i < threadCount; i++)
            {
                var thread = new Thread(async () => await this.MessageLoop().ConfigureAwait(false))
                {
                    IsBackground = true
                };
                thread.Start();
                this.threads.Add(thread);
            }

            this.logger.LogInformation("Worker Started. Threads: {ThreadCount}", threadCount);
        }

        /// <summary>
        /// This method is called to stop this worker.
        /// </summary>
        public void Stop()
        {
            this.active = false;
            this.threads.ForEach((t) => t.Join());
            this.threads = new List<Thread>();
        }

        /// <summary>
        /// This method is called to update correlation data from a received message.
        /// </summary>
        /// <param name="response">AwsMessageResponse</param>
        public void UpdateCorrelationData(AwsMessageResponse response)
        {
            this.correlationService.Request.Parent = response.Message.Correlation.Request;
            this.correlationService.Request.Origin = response.Message.Correlation.Span;
            this.correlationService.Session = response.Message.Correlation.Session;
            this.correlationService.Trace = response.Message.Correlation.Trace;
        }

        /// <summary>
        /// This method is called to handle retry logic for a message.
        /// </summary>
        /// <param name="message">The IMessage.</param>
        /// <param name="args">The IMessageArgs related to the message.</param>
        public async System.Threading.Tasks.Task HandleRetry(IMessage message, IMessageArgs args)
        {
            var retryPolicy = this.Queue.RetryPolicy;

            // Check if the message has reached max retries allowed
            if (args.RetryAttempts >= retryPolicy.MaxRetries)
            {
                if (this.Queue.EnableDlq)
                {
                    await this.queueService.SendToDlqAsync(message, this.Queue).ConfigureAwait(false);
                }

                // remove message from queue
                await this.queueService.DeleteMessageAsync(this.Queue, args.ReceiptHandle).ConfigureAwait(false);
            }
            else
            {
                // calculate the retry delay
                int timeout = retryPolicy.Delay;
                if (retryPolicy.BackOff)
                {
                    // incremental backoff is enabled so calculate based on retry attempts
                    int backoffCount = args.RetryAttempts + 1;
                    timeout = retryPolicy.Delay * backoffCount;
                }

                // change the visibility timeout of the message to match retry timeout
                await this.queueService.ChangeMessageVisibilityAsync(this.Queue, args.ReceiptHandle, timeout).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// This method is called to get the current state of this queue worker.
        /// </summary>
        public QueueWorkerState CurrentState()
        {
            if (!this.active)
            {
                return QueueWorkerState.Idle;
            }

            if (this.threads.Find(t =>
                   t.ThreadState != ThreadState.Running
                   || t.ThreadState != ThreadState.WaitSleepJoin
                   || t.ThreadState != ThreadState.Background) == null)
            {
                return QueueWorkerState.Unhealthy;
            }

            return QueueWorkerState.Healthy;
        }

        /// <summary>
        /// This method is called to determine the number of threads to use for processing messages.
        /// </summary>
        public int ThreadCount()
        {
            string? value = Environment.GetEnvironmentVariable("QUEUE_WORKER_THREADS");
            if (value == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(value);
            }
        }

        /// <summary>
        /// This method is called to determine if a specific topic has been registered or not.
        /// </summary>
        /// <typeparam name="T">The type of the ITopic</typeparam>
        public bool IsTopicRegistered<T>()
        where T : ITopic
        {
            return this.topicService.IsRegistered<T>();
        }

        /// <summary>
        /// This method is called to log an error
        /// </summary>
        /// <param name="ex">The exception that occurred</param>
        private void LogUnhandledError(System.Exception ex)
        {
            this.logger.LogError(ex, "An unhandled error occured. {Message}", ex.Message);
        }

        /// <summary>
        /// This method is called to handle the message loop for a thread.
        /// </summary>
        private async Task MessageLoop()
        {
            while (this.active)
            {
                this.correlationService.Reset();

                try
                {
                    var response = await this.queueService.ReceiveMessageAsync(this.Queue, this.WaitTime).ConfigureAwait(false);
                    if (response == null) continue;

                    this.UpdateCorrelationData(response);
                    this.middleware.ForEach(mw => mw.Invoke(response.Message, response.Args));

                    this.logger.LogInformation("[{Type}] - Message received from queue: {Name}.", this.GetType(), this.Queue.Name);

                    bool error = await this.ProcessMessageAsync(response);

                    await this.FinalizeMessageAsync(response, error).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    this.LogUnhandledError(ex);
                }
            }
        }

        private async Task<bool> ProcessMessageAsync(AwsMessageResponse response)
        {
            if (response.Args.Aborted || response.Args.Failed)
            {
                return false;
            }

            try
            {
                var availableHandlers = this.messageHandlers
                    .Where(h => h.CanProcess(response.Message, response.Args))
                    .ToList();

                if (availableHandlers.Count == 0)
                {
                    this.logger.LogWarning("[{Type}] - No message handler found for message. Topic: {Topic}",
                        this.GetType(), response.Message.Topic ?? "Unknown");
                    return false;
                }

                foreach (IMessageHandler handler in availableHandlers)
                {
                    await this.InvokeHandlerAsync(handler, response).ConfigureAwait(false);
                }

                return false;
            }
            catch (Exception ex)
            {
                this.LogUnhandledError(ex);
                await Task.WhenAll(this.errorHandlers.Select(h => h.OnError(ex, response.Message, response.Args)));
                return true;
            }
        }

        private async Task InvokeHandlerAsync(IMessageHandler handler, AwsMessageResponse response)
        {
            string handlerTypeName = handler.GetType().ToString();
            using (this.traceService.Trace($"[{handlerTypeName}] - ProcessMessage.", handlerTypeName, "MessageReceived"))
            {
                await handler.ProcessMessage(response.Message, response.Args).ConfigureAwait(false);
            }
        }

        private async Task FinalizeMessageAsync(AwsMessageResponse response, bool error)
        {
            if (response.Args.Aborted || error)
            {
                await this.HandleRetry(response.Message, response.Args).ConfigureAwait(false);
                return;
            }

            if (response.Args.Failed)
            {
                this.logger.LogWarning("[{Type}] - Sending message to DLQ", this.GetType());
                await this.queueService.SendToDlqAsync(response.Message, this.Queue).ConfigureAwait(false);
            }

            await this.queueService.DeleteMessageAsync(this.Queue, response.Args.ReceiptHandle).ConfigureAwait(false);
        }

    }
}
