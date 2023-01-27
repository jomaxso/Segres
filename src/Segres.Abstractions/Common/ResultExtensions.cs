using System.ComponentModel.DataAnnotations;

namespace Segres;

/// <summary>
/// Extensions for the <see cref="Result{T}"/> class.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Wraps a value.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <typeparam name="T">The expected value type of the result.</typeparam>
    /// <returns>A result wrapping an value.</returns>
    public static Result<T> AsResult<T>(this T value) => value;
    
    /// <summary>
    /// Wraps a exception to prevent throwing it.
    /// </summary>
    /// <param name="exception">The exception to wrap.</param>
    /// <typeparam name="T">The expected value type of the result.</typeparam>
    /// <returns>A result wrapping an exception.</returns>
    public static Result<T> AsResult<T>(this Exception exception) => exception;

    /// <summary>
    /// Validate a result object by the predicate and returns a result or an error.
    /// </summary>
    /// <param name="result">The result to validate</param>
    /// <param name="condition">The condition to validate.</param>
    /// <param name="error">The error to return in case of a failing condition.</param>
    /// <returns>The result after validating.</returns>
    public static Result<T> Ensure<T>(
        this Result<T> result,
        Func<T, bool> condition,
        Exception? error = null)
    {
        if (result.TryGetValue(out var value) is false)
            return result!;

        if (condition(value!))
            return result!;
        
        return error ?? new ValidationException($"the validation for {typeof(T)} failed.");
    }
}