namespace MicrolisR.Validation;

public interface IValidation
{
    internal void Validate(IValidatable value);
}

public interface IValidation<in TObject> : IValidation
    where TObject : IValidatable
{

    void IValidation.Validate(IValidatable value)
    {
        if (value is TObject validatable)
            Validate(validatable);
    }
    
    public void Validate(TObject value);
}