namespace Segres.Commons;

public readonly record struct Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error Null = new("Error.Null", "The specified result is null.");

    public static implicit operator string(Error error) => error.Code;
}