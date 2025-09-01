namespace Avvo.Core.Messaging
{
    public enum QueueWorkerState
    {
        /// This specifies a Queue Worker is active and healthy.
        Healthy,

        /// This specifies a Queue Worker is active but unhealthy.
        Unhealthy,

        /// This specifies a Queue Worker is not active.
        Idle
    }
}
