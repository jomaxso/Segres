using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Segres;

/// <summary>
/// A wrapper to represent a result. 
/// </summary>
/// <typeparam name="T">The type of the result to wrap.</typeparam>
public readonly struct Result<T> : IEquatable<Result<T>>
{
    private readonly ResultState _state = ResultState.Invalid;
    private readonly Exception? _exception;
    private readonly T? _value;

    public Result()
    {
        _state = ResultState.Invalid;
        _value = default;
        _exception = default;
        throw new InvalidOperationException("Please call an other constructor.");
    }

    public Result(T value)
    {
        _state = ResultState.Success;
        _value = value;
        _exception = null;
    }

    public Result(Exception exception)
    {
        _state = ResultState.Failed;
        _value = default;
        _exception = exception;
    }

    /// <summary>
    /// Indicates if true, that this result has a value.
    /// </summary>
    [Pure]
    public bool IsSuccess => _state is ResultState.Success;

    /// <summary>
    /// Indicates if true, that this result has an error.
    /// </summary>
    [Pure]
    public bool IsFailure => _state is ResultState.Failed;

    /// <summary>
    /// Get the value of this result.
    /// </summary>
    /// <returns>The value.</returns>
    /// <exception cref="Exception">When in a failure state.</exception>
    /// <exception cref="UnexpectedResultStateException">When in a invalid state.</exception>
    [Pure]
    public T GetValue() => _state switch
    {
        ResultState.Success => _value!,
        ResultState.Failed => throw _exception!,
        ResultState.Invalid => throw new UnexpectedResultStateException(),
        _ => throw new UnreachableException()
    };

    /// <summary>
    /// Try to get the value of this result.
    /// </summary>
    /// <param name="value">The value oft this result.</param>
    /// <returns>true if in a success state.</returns>
    [Pure]
    public bool TryGetValue(out T? value)
    {
        value = _value;
        return IsSuccess;
    }

    /// <summary>
    /// Get the error of this result.
    /// </summary>
    /// <returns>The error of this result.</returns>
    [Pure]
    public Exception? GetError() => _exception;

    /// <summary>
    /// Resolve this result in case of a success and failure state.
    /// </summary>
    /// <param name="success">Handle the success state.</param>
    /// <param name="failure">Handle the failure state.</param>
    /// <typeparam name="TResult">Type of the expected value.</typeparam>
    /// <returns>The expected value.</returns>
    public TResult Match<TResult>(Func<T, TResult> success, Func<Exception, TResult> failure)
        => IsSuccess ? success(_value!) : failure(_exception!);

    [Pure]
    public static implicit operator Result<T>(T value) => new(value);

    [Pure]
    public static implicit operator T(Result<T> result) => result.GetValue();

    [Pure]
    public static implicit operator Result<T>(Exception exception) => new(exception);

    [Pure]
    public static implicit operator Exception?(Result<T> result) => result.GetError();

    [Pure]
    public static bool operator ==(Result<T> a, Result<T> b) => a.Equals(b);

    [Pure]
    public static bool operator !=(Result<T> a, Result<T> b) => !(a == b);

    [Pure]
    public override bool Equals(object? obj) => obj is Result<T> other && Equals(other);

    [Pure]
    public bool Equals(Result<T> other)
    {
        return other._value?.Equals(_value) is true &&
               other._exception?.Equals(_exception) is true;
    }

    [Pure]
    public override int GetHashCode() => _value?.GetHashCode() + _exception?.GetHashCode() ?? 0;
}