namespace Avvo.Core.Logging.Correlation
{
    using System;

    /// <summary>
    /// This class is used to define a request header to pass correlation data in a web request.
    /// </summary>
    public class CorrelationRequestHeader
    {
        /// <summary>
        /// This is the unique identifier of the trace that sent the request.
        /// </summary>
        public Guid Trace { get; set; }

        /// <summary>
        /// This is the unique identifier of the request that sent the the request.
        /// </summary>
        public Guid Request { get; set; }

        /// <summary>
        /// This is the unique identifier of the span that sent the the request.
        /// </summary>
        public Guid Span { get; set; }

        /// <summary>
        /// This is the unique identifier of the session that sent the request.
        /// </summary>
        public Guid Session { get; set; }
    }
}
