namespace MicrolisR.Validation;

public interface IValidationContext<out TObject>
{
    internal TObject Value { get; }
    
    internal string FieldName { get; }
}