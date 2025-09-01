using Avvo.Core.Commons.Consts;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Commons.Exceptions;

public class ServiceException : ExceptionBase
{
    public ServiceException(string message, Exception? ex = null, LogLevel logLevel = Microsoft.Extensions.Logging.LogLevel.Error)
    : base(message, ex, ExceptionLayers.Service, logLevel)
    {

    }
}
