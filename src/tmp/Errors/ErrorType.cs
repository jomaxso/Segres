namespace MicrolisR.Errors;

public enum ErrorType : byte
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
}