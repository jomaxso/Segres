namespace MicrolisR.Errors;

public readonly record struct Result
{
    public static Result Empty { get; } = new();

    public Result(Exception exception)
    {
        IsError = true;
        Error = new Error("", "", ErrorType.Unexpected, new Exception("Error in result", exception));
    }

    public Result(Error error)
    {
        IsError = true;
        Error = error;
    }

    public bool IsError { get; }
    public Error? Error { get; }
}

public readonly record struct Result<TValue>
{
    public static Result<TValue> Empty { get; } = new();

    public Result(TValue value)
    {
        Value = value;
        Error = null;
        IsError = false;
    }

    public Result(Exception exception)
    {
        IsError = true;
        Value = default;
        Error = new Error("", "", ErrorType.Unexpected, new Exception("Error in result", exception));
    }

    public Result(Error error)
    {
        IsError = true;
        Value = default;
        Error = error;
    }

    public bool IsError { get; }
    public TValue? Value { get; }
    public Error? Error { get; }

    public static implicit operator TValue(Result<TValue> result)
    {
        if (result.IsError)
            ((Error) result.Error!).Throw();

        return result.Value!;
    }

    public static implicit operator Error?(Result<TValue> result)
        => result.Error;

    public static implicit operator Result<TValue>(TValue result)
        => new(result);

    public static implicit operator Result<TValue>(Exception exception)
        => new(exception);

    public static implicit operator Result<TValue>(Error error)
        => new(error);
}