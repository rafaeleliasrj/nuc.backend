namespace Avvo.Core.Logging
{
    using System.Collections.Generic;
    using System;
    using Microsoft.Extensions.Logging;
    using Avvo.Core.Logging.Correlation;

    /// <summary>
    /// This is the custom logger provider used to create the Logger class.
    /// </summary>
    public class LoggerProvider : ILoggerProvider
    {
        static Dictionary<string, ILogger> logStore = new Dictionary<string, ILogger>();

        private ICorrelationService correlationService;

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="correlationService">The ICorrelationService to use</param>
        public LoggerProvider(ICorrelationService correlationService)
        {
            this.correlationService = correlationService;
        }

        /// <summary>
        /// This method is called to dispose this provider
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// This method is called to create an ILogger instance
        /// </summary>
        /// <param name="categoryName">The category of the ILogger</param>
        public ILogger CreateLogger(string categoryName)
        {
            ILogger logger = null;

            if (!logStore.ContainsKey(categoryName))
            {
                logger = new Logger(categoryName, this.correlationService, new ApplicationDetails());
                logStore.Add(categoryName, logger);
            }
            else
            {
                logger = logStore[categoryName];
            }

            return logger;
        }
    }
}
