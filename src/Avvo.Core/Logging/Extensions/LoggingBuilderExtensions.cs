namespace Avvo.Core.Logging.Extensions
{
    using Microsoft.Extensions.Logging;
    using Avvo.Core.Logging.Correlation;

    /// <summary>
    /// This class is used to add extension methods to the ILoggingBuilder
    /// </summary>
    public static class LoggingBuilderExtensions
    {
        /// <summary>
        /// This method is called to add the Custom Logger Provider to the Logger Factory.
        /// </summary>
        /// <param name="loggingBuilder">The owner of this extension method.</param>
        /// <param name="correlationService">The correlation service to pass to the Logger</param>
        /// <returns></returns>
        public static Microsoft.Extensions.Logging.ILoggingBuilder AddCustomLogger(this Microsoft.Extensions.Logging.ILoggingBuilder loggingBuilder, ICorrelationService correlationService)
        {
            return loggingBuilder.AddProvider(new LoggerProvider(correlationService));
        }
    }
}
