namespace Avvo.Core.Messaging.Interface
{
    /// <summary>
    /// This interface defines a message queue worker.
    /// </summary>
    public interface IQueueWorker
    {
        /// <summary>
        /// This method is called to register the queue this worker should use.
        /// </summary>
        /// <param name="queue">The message queue this worker should receive messages from.</param>
        void RegisterQueue(IQueue queue);

        /// <summary>
        /// This method is called to register the queue this worker should use.
        /// </summary>
        /// <typeparam name="T">The IQueue type that this worker should receive messages from.</typeparam>
        void RegisterQueue<T>()
        where T : IQueue;

        /// <summary>
        /// This method is called to register an error handler with this worker.
        /// </summary>
        /// <typeparam name="T">The IErrorHandler to register.</typeparam>
        void RegisterErrorHandler<T>()
        where T : IErrorHandler;

        /// <summary>
        /// This method is called to determine if a specific error handler has been registered.
        /// </summary>
        /// <typeparam name="T">The IErrorHandler type</typeparam>
        bool IsErrorHandlerRegistered<T>()
        where T : IErrorHandler;

        /// <summary>
        /// This method is called to register a topic this worker should subscribe to.
        /// </summary>
        /// <typeparam name="T">The type of the ITopic to subscribe to.</typeparam>
        void RegisterTopic<T>()
        where T : ITopic;

        /// <summary>
        /// This method is called to register an IMessageHandler with this worker.
        /// </summary>
        /// <typeparam name="T">The IMessageHandler to register.</typeparam>
        void RegisterMessageHandler<T>()
        where T : IMessageHandler;

        /// <summary>
        /// This method is called to determine if a specific message handler has been registered.
        /// </summary>
        /// <typeparam name="T">The IMessageHandler type</typeparam>
        bool IsMessageHandlerRegistered<T>()
        where T : IMessageHandler;

        /// <summary>
        /// This method is called to register an IMessageMiddleware with this worker.
        /// </summary>
        /// <typeparam name="T">The IMessageMiddleware to register.</typeparam>
        void RegisterMiddleware<T>()
        where T : IMessageMiddleware;

        /// <summary>
        /// This method is called to determine if a specific middleware has been registered.
        /// </summary>
        /// <typeparam name="T">The IMessageMiddleware type</typeparam>
        bool IsMiddlewareRegistered<T>()
        where T : IMessageMiddleware;

        /// <summary>
        /// This method is called to register a topic this worker should subscribe to.
        /// </summary>
        /// <param name="topic">The ITopic to subscribe to</param>
        void RegisterTopic(ITopic topic);

        /// <summary>
        /// This method is called to determine if a specific topic has been registered.
        /// </summary>
        /// <typeparam name="T">The type of the ITopic</typeparam>
        bool IsTopicRegistered<T>()
        where T : ITopic;

        /// <summary>
        /// This method is called to start this worker.
        /// </summary>
        void Start();

        /// <summary>
        /// This method is called to stop this worker.
        /// </summary>
        void Stop();

        /// <summary>
        /// This method is called to get the current state of this worker.
        /// </summary>
        QueueWorkerState CurrentState();

        /// <summary>
        /// This method is called to determine the number of threads to use for processing messages.
        /// </summary>
        int ThreadCount();
    }
}
