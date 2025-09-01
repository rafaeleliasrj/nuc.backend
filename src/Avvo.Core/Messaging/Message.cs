using Avvo.Core.Logging.Correlation;
using Avvo.Core.Messaging.Interface;
using Newtonsoft.Json.Linq;

namespace Avvo.Core.Messaging
{
    public class Message : IMessage
    {
        /// <summary>
        /// This is the unique identifier of this message.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// This is the topic this message was published to.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// This is the content of this message.
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// This is the UTC DateTime this message was published.
        /// </summary>
        public DateTime PublishedAt { get; set; }

        /// <summary>
        /// This is the associated correlation data for this message.
        /// </summary>
        public CorrelationRequestHeader Correlation { get; set; }

        /// <summary>
        /// This is the default constructor
        /// </summary>
        public Message()
        {
            this.Id = Guid.NewGuid();
            this.PublishedAt = DateTime.UtcNow;
            this.Correlation = new CorrelationRequestHeader();
            this.Topic = string.Empty; // evita null
            this.Content = new object(); // placeholder seguro
            this.Identity = new Dictionary<string, string>();
        }


        /// <summary>
        /// This method is called to get the content of this message as a specific type.
        /// </summary>
        /// <typeparam name="T">The Type to convert the message content into.</typeparam>
        public T? GetContent<T>()
        {
            if (this.Content is JObject jObject)
            {
                return jObject.ToObject<T>();
            }

            return default;
        }


        /// <summary>
        /// Identificação de qual o usuário
        /// </summary>
        public Dictionary<string, string> Identity { get; set; }
    }
}
