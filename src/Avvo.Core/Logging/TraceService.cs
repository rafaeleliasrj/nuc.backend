namespace Avvo.Core.Logging
{
    using Microsoft.Extensions.Logging;
    using Avvo.Core.Logging.Correlation;

    /// <summary>
    /// This class is used to define a correlation tracer.
    /// </summary>
    public class TraceService : ITraceService
    {
        private readonly ILogger logger;
        private readonly ICorrelationService correlationService;

        /// <summary>
        /// This is the default constructur for this object.
        /// </summary>
        /// <param name="logger">This is ILogger to use.</param>
        /// <param name="correlationService">This is the ICorrelationService to use.</param>
        public TraceService(ILogger logger, ICorrelationService correlationService)
        {
            this.logger = logger;
            this.correlationService = correlationService;
        }

        /// <summary>
        /// This method is called to start a correlation trace.
        /// </summary>
        /// <param name="message">This is the log message for the trace.</param>
        /// <param name="name">This is the optional name of the trace.</param>
        /// <param name="category">This is the optional category of the trace.</param>
        /// <param name="subCategory">This is the optional sub-category of the trace.</param>
        /// <param name="debug">This is used to specify if the trace should only log when LogLevel is debug.</param>
        /// <returns>ITracer object</returns>
        public ITracer Trace(string message, string name = null, string category = null, string subCategory = null, bool debug = false)
        {
            return new Tracer(this.logger, this.correlationService, message, name, category, subCategory, debug);
        }
    }
}
