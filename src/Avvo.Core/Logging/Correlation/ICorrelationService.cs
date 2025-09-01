namespace Avvo.Core.Logging.Correlation
{
    using System;

    /// <summary>
    /// This interface is used to define the service used to access the current correlation tracking data.
    /// </summary>
    public interface ICorrelationService
    {
        /// <summary>
        /// The correlation request data.
        /// </summary>
        RequestData Request { get; set; }

        /// <summary>
        /// The correlation trace id.
        /// </summary>
        Guid Trace { get; set; }

        /// <summary>
        /// The correlation span id.
        /// </summary>
        Guid Span { get; set; }

        /// <summary>
        /// The unique identifier of the current correlation session.
        /// </summary>
        Guid Session { get; set; }

        /// <summary>
        /// This method is called to reset the correlation data for the current context.
        /// </summary>
        void Reset();

        /// <summary>
        /// This method is called to load the correlation data for the current context from a correlation request header.
        /// </summary>
        /// <param name="requestHeader">The correlation request header to load</param>
        void Load(CorrelationRequestHeader requestHeader);

        /// <summary>
        /// This method is called to create a correlation request header for the current context.
        /// </summary>
        CorrelationRequestHeader CreateRequestHeader();
    }
}
