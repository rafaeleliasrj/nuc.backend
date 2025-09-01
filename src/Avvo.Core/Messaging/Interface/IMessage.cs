using System;
using System.Collections.Generic;
using Avvo.Core.Logging.Correlation;

namespace Avvo.Core.Messaging.Interface
{
    /// <summary>
    /// This interface defines a queue message.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// This is the unique identifier of the message.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// This is the name of the topic the message was published to.
        /// </summary>
        string Topic { get; }

        /// <summary>
        /// This is the message contents.
        /// </summary>
        object Content { get; }

        /// <summary>
        /// This is the DateTime the message was published.
        /// </summary>
        DateTime PublishedAt { get; }

        /// <summary>
        /// This is the correlation data related to this message.
        /// </summary>
        CorrelationRequestHeader Correlation { get; }

        /// <summary>
        /// Identificação do usuário que solicitou a criação
        /// desta mensagem.
        /// </summary>
        Dictionary<string, string> Identity { get; }

        /// <summary>
        /// This method is called to get the content of this message as a specific type.
        /// </summary>
        /// <typeparam name="T">The Type to convert the message content into.</typeparam>
        T? GetContent<T>();
    }
}
