using Avvo.Core.Commons.Consts;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Commons.Exceptions;

public class DataBaseException : ExceptionBase
{
    public DataBaseException(string message, Exception ex, LogLevel logLevel = LogLevel.Error)
    : base(message, ex, ExceptionLayers.Data, logLevel)
    {

    }
}
