using System.Globalization;
using Avvo.Core.Commons.Utils;
using Avvo.Core.Logging;
using Avvo.Core.Logging.Correlation;
using Avvo.Domain.Utils;

namespace Avvo.API
{
    /// <summary>
    /// Host Static Configs
    /// </summary>
    public class ApplicationConfiguration
    {
        /// <summary>
        ///     Service Name
        /// </summary>
        /// <returns></returns>
        public static string ServiceName => EnvironmentVariables.Get("APPLICATION_NAME");

        /// <summary>
        ///     Service Version
        /// </summary>
        /// <returns></returns>
        public static string ServiceVersion => EnvironmentVariables.Get("VERSION");

        /// <summary>
        ///     Port
        /// </summary>
        /// <value></value>
        public static string Port => EnvironmentVariables.Get("PORT");

        /// <summary>
        ///     Port
        /// </summary>
        /// <value></value>
        public static string Environment => EnvironmentVariables.Get("ENVIRONMENT");

        /// <summary>
        /// Logger
        /// </summary>
        private static ILogger logger;

        /// <summary>
        /// Correlation Service
        /// </summary>
        /// <value></value>
        public static ICorrelationService CorrelationService { get; } = new CorrelationService();

        /// <summary>
        ///     Logger
        /// </summary>
        /// <value></value>
        public static ILogger Logger
        {
            get
            {
                if (logger == null)
                {
                    using var loggerFactory = LoggerFactory.Create(ConfigureILoggingBuilder);
                    logger = loggerFactory.CreateLogger<ApplicationConfiguration>();
                }

                return logger;
            }
        }

        internal static void ConfigureILoggingBuilder(ILoggingBuilder builder)
        {
            builder.SetMinimumLevel(EnvironmentHelper.GetLogLevel());
            builder.AddProvider(new LoggerProvider(CorrelationService));
        }
    }
}
