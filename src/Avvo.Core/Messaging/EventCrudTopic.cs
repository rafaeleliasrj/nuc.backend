using Avvo.Core.Commons.Utils;
using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging
{
    public class CrudEventTopic : ITopic
    {
        public string Name { get { return EnvironmentVariables.Get("CRUD_EVENT_TOPIC_NAME"); } }
    }

}
