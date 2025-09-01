using Avvo.Core.Commons.Utils;
using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging
{
    public class CrudEventQueue : IQueue
    {
        public string Name { get { return EnvironmentVariables.Get("CRUD_EVENT_QUEUE_NAME"); } }

        public IRetryPolicy RetryPolicy { get { return new RetryPolicy(); } }

        public bool EnableDlq => false;
        public bool FifoQueue => false;

    }
}
