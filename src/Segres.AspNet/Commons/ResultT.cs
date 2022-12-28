namespace Segres.Commons;

public readonly record struct Result<TValue> 
{
    private readonly TValue? _value;
    private readonly Result _result;

    public bool IsSuccess => _result.IsSuccess;
    public Error Error => _result.Error;
    
    public Result() 
    {
        throw new InvalidOperationException();
    }
    
    internal Result(TValue? value, bool isSuccess, Error? error)
    {
        _value = value;
        _result = Result.Create(isSuccess, error);
    }
    
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => Result.Success(value);

    public static implicit operator Result(Result<TValue> value) => value._result;
}