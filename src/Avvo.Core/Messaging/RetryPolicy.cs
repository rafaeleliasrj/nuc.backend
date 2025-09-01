using Avvo.Core.Messaging.Interface;

namespace Avvo.Core.Messaging
{
    public class RetryPolicy : IRetryPolicy
    {
        /// <summary>
        /// This is the retry delay in seconds.
        /// When Backoff = true the retry inteval = Delay * RetryAttempts.
        /// When BackOff = false the retry interval = Delay
        /// </summary>
        public int Delay { get; protected set; }

        /// <summary>
        /// Should retries apply an incremental backoff.
        /// </summary>
        public bool BackOff { get; protected set; }

        /// <summary>
        /// The maximum number of times a message should be allowed to be retried.
        /// </summary>
        public int MaxRetries { get; protected set; }

        /// <summary>
        /// This is the default constructor.
        /// </summary>
        public RetryPolicy()
        {
            this.BackOff = false;
            this.MaxRetries = 300;
            this.Delay = 1;
        }

        /// <summary>
        /// This constructor is used to set values for this retry policy.
        /// </summary>
        /// <param name="backOff">Is incremental backoff enabled</param>
        /// <param name="maxRetries">The max number of retries to allow</param>
        /// <param name="delay">The delay between retries in seconds</param>
        public RetryPolicy(bool backOff, int maxRetries, int delay)
        {
            this.BackOff = backOff;
            this.MaxRetries = maxRetries;
            this.Delay = delay;
        }
    }
}
