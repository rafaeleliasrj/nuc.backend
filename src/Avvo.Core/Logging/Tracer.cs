namespace Avvo.Core.Logging
{
    using System;
    using Microsoft.Extensions.Logging;
    using Avvo.Core.Logging.Correlation;

    /// <summary>
    /// This class is used to define a correlation tracer.
    /// </summary>
    public class Tracer : ITracer, IDisposable
    {
        private readonly ILogger logger;
        private readonly ICorrelationService correlationService;
        private readonly SpanData parentSpan;
        private readonly string message;
        private readonly bool debug;
        private readonly SpanData spanData;

        /// <summary>
        /// This is the log message for this trace.
        /// </summary>
        public string Message { get { return this.message; } }

        /// <summary>
        /// This specifies if this Trace only logs when LogLevel is debug.
        /// </summary>
        public bool Debug { get { return this.debug; } }

        /// <summary>
        /// This is the current data for this span.
        /// </summary>
        public SpanData Data { get { return this.spanData; } }

        /// <summary>
        /// This is the constructor used to create a Tracer.
        /// </summary>
        /// <param name="logger">This is the ILogger instance to use.</param>
        /// <param name="correlationService">This is the ICorrelationService instance to use.</param>
        /// <param name="message">This is the trace message to log.</param>
        /// <param name="name">This is the optional name of the trace</param>
        /// <param name="category">This is the optional category of the trace</param>
        /// <param name="subCategory">This is the optional sub-category of the trace</param>
        /// <param name="debug">This is used to specify if the trace should be logged as a debug log entry.</param>
        public Tracer(
            ILogger logger,
            ICorrelationService correlationService,
            string message,
            string name = null,
            string category = null,
            string subCategory = null,
            bool debug = false
        )
        {
            this.logger = logger;
            this.correlationService = correlationService;
            this.parentSpan = SpanData.Current;
            this.message = message;
            this.debug = debug;
            this.spanData = new SpanData
            {
                Name = name,
                Category = category,
                SubCategory = subCategory,
                Started = DateTime.UtcNow
            };

            SpanData.Current = this.spanData;
            this.correlationService.Span = this.spanData.Id;
        }

        public void Dispose()
        {
            this.spanData.Completed = DateTime.UtcNow;

            if (this.debug)
            {
                this.logger.LogDebug(this.message);
            }
            else
            {
                this.logger.LogInformation(this.message);
            }

            SpanData.Current = this.parentSpan;
            this.correlationService.Span = this.parentSpan.Id;
        }
    }
}
