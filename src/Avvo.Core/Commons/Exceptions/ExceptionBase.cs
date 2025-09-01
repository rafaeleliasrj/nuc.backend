using Avvo.Core.Commons.Consts;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Commons.Exceptions;

public class ExceptionBase : Exception
{
    public readonly ExceptionLayers? Layer;
    public readonly LogLevel LogLevel;

    public ExceptionBase(LogLevel logLevel = LogLevel.Error)
    {
        LogLevel = logLevel;
    }

    public ExceptionBase(string message) : base(message)
    {
    }

    public ExceptionBase(string message,
                         LogLevel logLevel) : base(message)
    {
        LogLevel = logLevel;
    }

    public ExceptionBase(string message, Exception ex) : base(message, ex)
    {

    }

    public ExceptionBase(string message, Exception ex, LogLevel logLevel = LogLevel.Error) : base(message, ex)
    {
        LogLevel = logLevel;
    }

    public ExceptionBase(string message, ExceptionLayers layer, LogLevel logLevel = LogLevel.Error) : base(message)
    {
        Layer = layer;
        LogLevel = logLevel;
    }

    public ExceptionBase(string message, Exception ex, ExceptionLayers layer, LogLevel logLevel = LogLevel.Error) : base(message, ex)
    {
        Layer = layer;
        LogLevel = logLevel;
    }
}
