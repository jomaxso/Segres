using Microsoft.Extensions.Logging;

namespace Logging;

public abstract class LoggerBase : MircrolisR.Logging.ILogger
{
    private readonly ILogger _logger;

    public MircrolisR.Logging.DebugLogger Debug { get; }
    public MircrolisR.Logging.ReleaseLogger Release { get; }

    public LoggerBase(ILogger logger)
    {
        if (logger is null)
            throw new ArgumentNullException(nameof(logger));

        _logger = logger;
        Debug = new MircrolisR.Logging.DebugLogger(this);
        Release = new MircrolisR.Logging.ReleaseLogger(this);
    }

    public void Log(string message)
    {
        _logger.Log(LogLevel.None, message);
    }

    public void Log<T0>(string message, T0 arg0)
    {
        _logger.Log(LogLevel.None, message, arg0);
    }

    public void Log<T0, T1>(string message, T0 arg0, T1 arg1)
    {
        _logger.Log(LogLevel.None, message, arg0, arg1);
    }

    public void Log<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
    {
        _logger.Log(LogLevel.None, message, arg0, arg1, arg2);
    }

    public void Log<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        _logger.Log(LogLevel.None, message, arg0, arg1, arg2, arg3);
    }

    public void Log<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        _logger.Log(LogLevel.None, message, arg0, arg1, arg2, arg3, arg4);
    }

    public void Log<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        _logger.Log(LogLevel.None, message, arg0, arg1, arg2, arg3, arg4, arg5);
    }

    public void Log(string message, params object?[] args)
    {
        _logger.Log(LogLevel.None, message, args);
    }

    public void LogDebug(string message)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(message);
    }

    public void LogDebug<T0>(string message, T0 arg0)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(message, arg0);
    }

    public void LogDebug<T0, T1>(string message, T0 arg0, T1 arg1)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(message, arg0, arg1);
    }

    public void LogDebug<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(message, arg0, arg1, arg2);
    }

    public void LogDebug<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(message, arg0, arg1, arg2, arg3);
    }

    public void LogDebug<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(message, arg0, arg1, arg2, arg3, arg4);
    }

    public void LogDebug<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(message, arg0, arg1, arg2, arg3, arg4, arg5);
    }

    public void LogDebug(string message, params object?[] args)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(message, args);
    }

    public void LogTrace(string message)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
            _logger.LogTrace(message);
    }

    public void LogTrace<T0>(string message, T0 arg0)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
            _logger.LogTrace(message, arg0);
    }

    public void LogTrace<T0, T1>(string message, T0 arg0, T1 arg1)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
            _logger.LogTrace(message, arg0, arg1);
    }

    public void LogTrace<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
            _logger.LogTrace(message, arg0, arg1, arg2);
    }

    public void LogTrace<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
            _logger.LogTrace(message, arg0, arg1, arg2, arg3);
    }

    public void LogTrace<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
            _logger.LogTrace(message, arg0, arg1, arg2, arg3, arg4);
    }

    public void LogTrace<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
            _logger.LogTrace(message, arg0, arg1, arg2, arg3, arg4, arg5);
    }

    public void LogTrace(string message, params object?[] args)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
            _logger.LogTrace(message, args);
    }

    public void LogInformation(string message)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(message);
    }

    public void LogInformation<T0>(string message, T0 arg0)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(message, arg0);
    }

    public void LogInformation<T0, T1>(string message, T0 arg0, T1 arg1)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(message, arg0, arg1);
    }

    public void LogInformation<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(message, arg0, arg1, arg2);
    }

    public void LogInformation<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(message, arg0, arg1, arg2, arg3);
    }

    public void LogInformation<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(message, arg0, arg1, arg2, arg3, arg4);
    }

    public void LogInformation<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(message, arg0, arg1, arg2, arg3, arg4, arg5);
    }

    public void LogInformation(string message, params object?[] args)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(message, args);
    }

    public void LogWarning(string message)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(message);
    }

    public void LogWarning<T0>(string message, T0 arg0)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(message, arg0);
    }

    public void LogWarning<T0, T1>(string message, T0 arg0, T1 arg1)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(message, arg0, arg1);
    }

    public void LogWarning<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(message, arg0, arg1, arg2);
    }

    public void LogWarning<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(message, arg0, arg1, arg2, arg3);
    }

    public void LogWarning<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(message, arg0, arg1, arg2, arg3, arg4);
    }

    public void LogWarning<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(message, arg0, arg1, arg2, arg3, arg4, arg5);
    }

    public void LogWarning(string message, params object?[] args)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(message, args);
    }

    public void LogError(string message)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(message);
    }

    public void LogError<T0>(string message, T0 arg0)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(message, arg0);
    }

    public void LogError<T0, T1>(string message, T0 arg0, T1 arg1)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(message, arg0, arg1);
    }

    public void LogError<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(message, arg0, arg1, arg2);
    }

    public void LogError<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(message, arg0, arg1, arg2, arg3);
    }

    public void LogError<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(message, arg0, arg1, arg2, arg3, arg4);
    }

    public void LogError<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(message, arg0, arg1, arg2, arg3, arg4, arg5);
    }

    public void LogError(string message, params object?[] args)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(message, args);
    }

    public void LogCritical(string message)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(message);
    }

    public void LogCritical<T0>(string message, T0 arg0)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(message, arg0);
    }

    public void LogCritical<T0, T1>(string message, T0 arg0, T1 arg1)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(message, arg0, arg1);
    }

    public void LogCritical<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(message, arg0, arg1, arg2);
    }

    public void LogCritical<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(message, arg0, arg1, arg2, arg3);
    }

    public void LogCritical<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(message, arg0, arg1, arg2, arg3, arg4);
    }

    public void LogCritical<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(message, arg0, arg1, arg2, arg3, arg4, arg5);
    }

    public void LogCritical(string message, params object?[] args)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(message, args);
    }
}
