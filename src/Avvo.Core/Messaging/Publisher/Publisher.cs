using System.Collections.Generic;
using System.Threading.Tasks;
using Avvo.Core.Logging.Correlation;
using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging.Publisher
{
    public class Publisher : IPublisher
    {
        private readonly ICorrelationService _correlationService;

        private static Dictionary<(string topic, string queue), ITopicPublisher> _topicPublisher = new Dictionary<(string topic, string queue), ITopicPublisher>();

        public Publisher(ICorrelationService correlationService)
        {
            _correlationService = correlationService;
        }

        public async Task<PublishResponse> PublishAsync(object message, ITopic topic, IQueue queue, Dictionary<string, string> identity, bool useDefaultObjectMessage = false)
        {
            if (!_topicPublisher.TryGetValue((topic: topic.Name, queue: queue.Name), out var publisher))
            {
                publisher = await PublisherFactory.CreateAsync(_correlationService, topic, queue).ConfigureAwait(false);
                _topicPublisher.Add((topic: topic.Name, queue: queue.Name), publisher);
            }

            var publishResponse = await publisher.PublishAsync(topic, message, identity, useDefaultObjectMessage).ConfigureAwait(false);

            return publishResponse;
        }
    }

    public class Publisher<TMessage> : IPublisher<TMessage>
    {
        private readonly ICorrelationService _correlationService;
        public ITopic Topic { get; set; }
        public IQueue Queue { get; set; }

        private static ITopicPublisher _topicPublisher;

        public Publisher(ICorrelationService correlationService)
        {
            _correlationService = correlationService;
        }

        public async Task<PublishResponse> PublishAsync(TMessage message, Dictionary<string, string> identity, bool useDefaultObjectMessage = false)
        {
            if (_topicPublisher == null)
            {
                _topicPublisher = await PublisherFactory.CreateAsync(_correlationService, this.Topic, this.Queue).ConfigureAwait(false);
            }

            var publishResponse = await _topicPublisher.PublishAsync(this.Topic, message, identity, useDefaultObjectMessage).ConfigureAwait(false);

            return publishResponse;
        }

        public async Task<PublishResponse> PublishSQSAsync(TMessage message, Dictionary<string, string> identity, bool useDefaultObjectMessage = false)
        {
            if (_topicPublisher == null)
            {
                _topicPublisher = await PublisherFactory.CreateAsync(_correlationService, this.Topic, this.Queue).ConfigureAwait(false);
            }

            var publishResponse = await _topicPublisher.PublishAsync(this.Topic, message, identity, useDefaultObjectMessage).ConfigureAwait(false);

            return publishResponse;
        }
    }
}
