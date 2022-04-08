using System.Diagnostics;

namespace MircrolisR.Logging;

public sealed class ReleaseLogger
{
    private readonly ILogger _logger;

    internal ReleaseLogger(ILogger logger)
    {
        if (logger is null)
            throw new ArgumentNullException(nameof(logger));

        _logger = logger;
    }

    [Conditional("RELEASE")]
    public void Log(string message) => _logger.Log(message);

    [Conditional("RELEASE")]
    public void Log<T0>(string message, T0 arg0) => _logger.Log(message, arg0);

    [Conditional("RELEASE")]
    public void Log<T0, T1>(string message, T0 arg0, T1 arg1) => _logger.Log(message, arg0, arg1);

    [Conditional("RELEASE")]
    public void Log<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2) => _logger.Log(message, arg0, arg1, arg2);

    [Conditional("RELEASE")]
    public void Log(string message, params object?[] args) => _logger.Log(message, args);

    [Conditional("RELEASE")]
    public void LogDebug(string message) => _logger.LogDebug(message);

    [Conditional("RELEASE")]
    public void LogDebug<T0>(string message, T0 arg0) => _logger.LogDebug(message, arg0);

    [Conditional("RELEASE")]
    public void LogDebug<T0, T1>(string message, T0 arg0, T1 arg1) => _logger.LogDebug(message, arg0, arg1);

    [Conditional("RELEASE")]
    public void LogDebug<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2) => _logger.LogDebug(message, arg0, arg1, arg2);

    [Conditional("RELEASE")]
    public void LogDebug(string message, params object?[] args) => _logger.LogDebug(message, args);

    [Conditional("RELEASE")]
    public void LogTrace(string message) => _logger.LogTrace(message);

    [Conditional("RELEASE")]
    public void LogTrace<T0>(string message, T0 arg0) => _logger.LogTrace(message, arg0);

    [Conditional("RELEASE")]
    public void LogTrace<T0, T1>(string message, T0 arg0, T1 arg1) => _logger.LogTrace(message, arg0, arg1);

    [Conditional("RELEASE")]
    public void LogTrace<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2) => _logger.LogTrace(message, arg0, arg1, arg2);

    [Conditional("RELEASE")]
    public void LogTrace(string message, params object?[] args) => _logger.LogTrace(message, args);

    [Conditional("RELEASE")]
    public void LogInformation(string message) => _logger.LogInformation(message);

    [Conditional("RELEASE")]
    public void LogInformation<T0>(string message, T0 arg0) => _logger.LogInformation(message, arg0);

    [Conditional("RELEASE")]
    public void LogInformation<T0, T1>(string message, T0 arg0, T1 arg1) => _logger.LogInformation(message, arg0, arg1);

    [Conditional("RELEASE")]
    public void LogInformation<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2) => _logger.LogInformation(message, arg0, arg1, arg2);

    [Conditional("RELEASE")]
    public void LogInformation(string message, params object?[] args) => _logger.LogInformation(message, args);

    [Conditional("RELEASE")]
    public void LogWarning(string message) => _logger.LogWarning(message);

    [Conditional("RELEASE")]
    public void LogWarning<T0>(string message, T0 arg0) => _logger.LogWarning(message, arg0);

    [Conditional("RELEASE")]
    public void LogWarning<T0, T1>(string message, T0 arg0, T1 arg1) => _logger.LogWarning(message, arg0, arg1);

    [Conditional("RELEASE")]
    public void LogWarning<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2) => _logger.LogWarning(message, arg0, arg1, arg2);

    [Conditional("RELEASE")]
    public void LogWarning(string message, params object?[] args) => _logger.LogWarning(message, args);

    [Conditional("RELEASE")]
    public void LogError(string message) => _logger.LogError(message);

    [Conditional("RELEASE")]
    public void LogError<T0>(string message, T0 arg0) => _logger.LogError(message, arg0);

    [Conditional("RELEASE")]
    public void LogError<T0, T1>(string message, T0 arg0, T1 arg1) => _logger.LogError(message, arg0, arg1);

    [Conditional("RELEASE")]
    public void LogError<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2) => _logger.LogError(message, arg0, arg1, arg2);

    [Conditional("RELEASE")]
    public void LogError(string message, params object?[] args) => _logger.LogError(message, args);

    [Conditional("RELEASE")]
    public void LogCritical(string message) => _logger.LogCritical(message);

    [Conditional("RELEASE")]
    public void LogCritical<T0>(string message, T0 arg0) => _logger.LogCritical(message, arg0);

    [Conditional("RELEASE")]
    public void LogCritical<T0, T1>(string message, T0 arg0, T1 arg1) => _logger.LogCritical(message, arg0, arg1);

    [Conditional("RELEASE")]
    public void LogCritical<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2) => _logger.LogCritical(message, arg0, arg1, arg2);

    [Conditional("RELEASE")]
    public void LogCritical(string message, params object?[] args) => _logger.LogCritical(message, args);

}
