namespace Avvo.Core.Logging
{
    using System;
    using Avvo.Core.Logging.Correlation;

    /// <summary>
    /// This class is used to define the correlation data structure for a log entry.
    /// </summary>
    public class CorrelationData
    {
        /// <summary>
        /// This is the correlation request data.
        /// </summary>
        public RequestData Request { get; set; }

        /// <summary>
        /// This is the correlation session id.
        /// </summary>
        public Guid Session { get; set; }

        /// <summary>
        /// This is the correlation trace id.
        /// </summary>
        public Guid Trace { get; set; }

        /// <summary>
        /// This is the correlation span data.
        /// </summary>
        public SpanData Span { get; set; }
    }
}
