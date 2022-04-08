using Logging;

namespace MircrolisR.Logging;

public class Logger<T> : Logger, ILogger<T>
{
    public Logger(Microsoft.Extensions.Logging.ILogger<T> logger) : base(logger) { }
}

public class Logger : LoggerBase
{
    private readonly Microsoft.Extensions.Logging.ILogger _logger;

    public Logger(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
    {
        if (logger is null)
            throw new ArgumentNullException(nameof(logger));

        _logger = logger;
    }


}
