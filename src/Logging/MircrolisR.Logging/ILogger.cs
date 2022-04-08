namespace MircrolisR.Logging;

public interface ILogger<T> : ILogger { }
public interface ILogger
{
    DebugLogger Debug { get; }
    ReleaseLogger Release { get; }

    void Log(string message);
    void Log(string message, params object?[] args);
    void Log<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    void Log<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    void Log<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    void Log<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);
    void Log<T0, T1>(string message, T0 arg0, T1 arg1);
    void Log<T0>(string message, T0 arg0);
    void LogCritical(string message);
    void LogCritical(string message, params object?[] args);
    void LogCritical<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    void LogCritical<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    void LogCritical<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    void LogCritical<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);
    void LogCritical<T0, T1>(string message, T0 arg0, T1 arg1);
    void LogCritical<T0>(string message, T0 arg0);
    void LogDebug(string message);
    void LogDebug(string message, params object?[] args);
    void LogDebug<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    void LogDebug<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    void LogDebug<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    void LogDebug<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);
    void LogDebug<T0, T1>(string message, T0 arg0, T1 arg1);
    void LogDebug<T0>(string message, T0 arg0);
    void LogError(string message);
    void LogError(string message, params object?[] args);
    void LogError<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    void LogError<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    void LogError<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    void LogError<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);
    void LogError<T0, T1>(string message, T0 arg0, T1 arg1);
    void LogError<T0>(string message, T0 arg0);
    void LogInformation(string message);
    void LogInformation(string message, params object?[] args);
    void LogInformation<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    void LogInformation<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    void LogInformation<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    void LogInformation<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);
    void LogInformation<T0, T1>(string message, T0 arg0, T1 arg1);
    void LogInformation<T0>(string message, T0 arg0);
    void LogTrace(string message);
    void LogTrace(string message, params object?[] args);
    void LogTrace<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    void LogTrace<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    void LogTrace<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    void LogTrace<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);
    void LogTrace<T0, T1>(string message, T0 arg0, T1 arg1);
    void LogTrace<T0>(string message, T0 arg0);
    void LogWarning(string message);
    void LogWarning(string message, params object?[] args);
    void LogWarning<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    void LogWarning<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    void LogWarning<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    void LogWarning<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);
    void LogWarning<T0, T1>(string message, T0 arg0, T1 arg1);
    void LogWarning<T0>(string message, T0 arg0);
}
