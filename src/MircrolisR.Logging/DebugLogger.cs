using System.Diagnostics;

namespace MircrolisR.Logging;

public sealed class DebugLogger
{
    private readonly ILogger _logger;

    internal DebugLogger(ILogger logger)
    {
        if (logger is null)
            throw new ArgumentNullException(nameof(logger));

        _logger = logger;
    }

    [Conditional("DEBUG")]
    public void Log(string message) => _logger.Log(message);

    [Conditional("DEBUG")]
    public void Log<T0>(string message, T0 arg0) => _logger.Log(message, arg0);

    [Conditional("DEBUG")]
    public void Log<T0, T1>(string message, T0 arg0, T1 arg1) => _logger.Log(message, arg0, arg1);

    [Conditional("DEBUG")]
    public void Log<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2) => _logger.Log(message, arg0, arg1, arg2);

    [Conditional("DEBUG")]
    public void Log(string message, params object?[] args) => _logger.Log(message, args);

    [Conditional("DEBUG")]
    public void LogDebug(string message) => _logger.LogDebug(message);

    [Conditional("DEBUG")]
    public void LogDebug<T0>(string message, T0 arg0) => _logger.LogDebug(message, arg0);

    [Conditional("DEBUG")]
    public void LogDebug<T0, T1>(string message, T0 arg0, T1 arg1) => _logger.LogDebug(message, arg0, arg1);

    [Conditional("DEBUG")]
    public void LogDebug<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2) => _logger.LogDebug(message, arg0, arg1, arg2);

    [Conditional("DEBUG")]
    public void LogDebug(string message, params object?[] args) => _logger.LogDebug(message, args);

    [Conditional("DEBUG")]
    public void LogTrace(string message) => _logger.LogTrace(message);

    [Conditional("DEBUG")]
    public void LogTrace<T0>(string message, T0 arg0) => _logger.LogTrace(message, arg0);

    [Conditional("DEBUG")]
    public void LogTrace<T0, T1>(string message, T0 arg0, T1 arg1) => _logger.LogTrace(message, arg0, arg1);

    [Conditional("DEBUG")]
    public void LogTrace<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2) => _logger.LogTrace(message, arg0, arg1, arg2);

    [Conditional("DEBUG")]
    public void LogTrace(string message, params object?[] args) => _logger.LogTrace(message, args);

    [Conditional("DEBUG")]
    public void LogInformation(string message) => _logger.LogInformation(message);

    [Conditional("DEBUG")]
    public void LogInformation<T0>(string message, T0 arg0) => _logger.LogInformation(message, arg0);

    [Conditional("DEBUG")]
    public void LogInformation<T0, T1>(string message, T0 arg0, T1 arg1) => _logger.LogInformation(message, arg0, arg1);

    [Conditional("DEBUG")]
    public void LogInformation<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2) => _logger.LogInformation(message, arg0, arg1, arg2);

    [Conditional("DEBUG")]
    public void LogInformation(string message, params object?[] args) => _logger.LogInformation(message, args);

    [Conditional("DEBUG")]
    public void LogWarning(string message) => _logger.LogWarning(message);

    [Conditional("DEBUG")]
    public void LogWarning<T0>(string message, T0 arg0) => _logger.LogWarning(message, arg0);

    [Conditional("DEBUG")]
    public void LogWarning<T0, T1>(string message, T0 arg0, T1 arg1) => _logger.LogWarning(message, arg0, arg1);

    [Conditional("DEBUG")]
    public void LogWarning<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2) => _logger.LogWarning(message, arg0, arg1, arg2);

    [Conditional("DEBUG")]
    public void LogWarning(string message, params object?[] args) => _logger.LogWarning(message, args);

    [Conditional("DEBUG")]
    public void LogError(string message) => _logger.LogError(message);

    [Conditional("DEBUG")]
    public void LogError<T0>(string message, T0 arg0) => _logger.LogError(message, arg0);

    [Conditional("DEBUG")]
    public void LogError<T0, T1>(string message, T0 arg0, T1 arg1) => _logger.LogError(message, arg0, arg1);

    [Conditional("DEBUG")]
    public void LogError<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2) => _logger.LogError(message, arg0, arg1, arg2);

    [Conditional("DEBUG")]
    public void LogError(string message, params object?[] args) => _logger.LogError(message, args);

    [Conditional("DEBUG")]
    public void LogCritical(string message) => _logger.LogCritical(message);

    [Conditional("DEBUG")]
    public void LogCritical<T0>(string message, T0 arg0) => _logger.LogCritical(message, arg0);

    [Conditional("DEBUG")]
    public void LogCritical<T0, T1>(string message, T0 arg0, T1 arg1) => _logger.LogCritical(message, arg0, arg1);

    [Conditional("DEBUG")]
    public void LogCritical<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2) => _logger.LogCritical(message, arg0, arg1, arg2);

    [Conditional("DEBUG")]
    public void LogCritical(string message, params object?[] args) => _logger.LogCritical(message, args);

}
