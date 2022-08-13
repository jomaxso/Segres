namespace MicrolisR.Validation;

internal readonly record struct ValidationContext<TObject> : IValidationContext<TObject>
{
    private readonly TObject _value;
    private readonly string _fieldName;

    internal ValidationContext(TObject value, ref string fieldName)
    {
        _value = value;
        _fieldName = fieldName;
    }

    public static implicit operator TObject(ValidationContext<TObject> context) => context._value;
    
    TObject IValidationContext<TObject>.Value => _value;

    string IValidationContext<TObject>.FieldName => _fieldName;
};