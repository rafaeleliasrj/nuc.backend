using Avvo.Core.Commons.Consts;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Commons.Exceptions;

public class NotFoundException : ExceptionBase
{
    public NotFoundException(string message) : base(message, ExceptionLayers.Service)
    {

    }

    public NotFoundException(string message, ExceptionLayers layer) : base(message, layer)
    {

    }

    public NotFoundException(string message, ExceptionLayers layer, LogLevel logLevel) : base(message, layer, logLevel)
    {

    }
}
