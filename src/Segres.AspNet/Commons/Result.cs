namespace Segres.Commons;


public readonly record struct Result
{
    public Result() 
    {
        throw new InvalidOperationException();
    }
    
    private Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None) 
            throw new InvalidOperationException();

        if (!isSuccess && error == Error.None) 
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result<TValue> Success<TValue>(TValue? value) => new(value, true, Error.None);
    
    public static Result Failure(Error error) => new(false, error);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    
    public static Result Create(bool isSuccess, Error? error) => new(isSuccess, error ?? Error.None);
    public static Result<TValue> Create<TValue>(TValue? value, bool isSuccess, Error? error) =>  new(value, isSuccess, error);
}

