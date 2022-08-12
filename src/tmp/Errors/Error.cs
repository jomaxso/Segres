namespace MicrolisR.Errors;

public readonly record struct Error
{
    private readonly Exception _exception;
    
    internal Error(string code, string description, ErrorType errorType, Exception exception)
    {
        Code = code;
        Description = description;
        ErrorType = errorType;
        _exception = exception;
    }
    
    public string Code { get; }
    
    public string Description { get; }
    
    public ErrorType ErrorType { get; }

    public void Throw() => throw new Exception(string.Empty, _exception.InnerException);
}