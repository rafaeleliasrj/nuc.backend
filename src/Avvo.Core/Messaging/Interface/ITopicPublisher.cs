using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avvo.Core.Messaging.Interface
{
    /// <summary>
    /// Define um Publisher que publica mensagem em um Topic espec�fico.
    /// </summary>
    public interface ITopicPublisher
    {
        /// <summary>
        /// Envia uma mensagem para um t�pico SNS.
        /// </summary>
        /// <param name="topic">The topic to publish the message to.</param>
        /// <param name="message">The message to publish.</param>
        /// <param name="identity"></param>
        /// <param name="useDefaultObjectMessage"></param>
        Task<PublishResponse> PublishAsync(ITopic topic, object message, Dictionary<string, string> identity, bool useDefaultObjectMessage);
    }
}
