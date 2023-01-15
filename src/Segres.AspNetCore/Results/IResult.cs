namespace Segres.AspNetCore;

/// <summary>
/// Represents a common result. 
/// </summary>
/// <typeparam name="TSelf">The actual result.</typeparam>
/// <typeparam name="T">The value to be wrapped in a result.</typeparam>
public interface IResult<out TSelf, T>
{
    /// <summary>
    /// Creates a new result instance.
    /// </summary>
    /// <param name="result">The value to be wrapped.</param>
    /// <returns>The actual, newly created result.</returns>
    static abstract TSelf Create(T result);
    public T Result { get; }
}