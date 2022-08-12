using System.Runtime.CompilerServices;

namespace MicrolisR.Validation;

public readonly record struct ValidationContext<T>
{
    internal ValidationContext(T value, string fieldName)
    {
        Value = value;
        FieldName = fieldName;
    }

    public T Value { get; }
    public string FieldName { get; }
    
    public static implicit operator T(ValidationContext<T> context) => context.Value;
};