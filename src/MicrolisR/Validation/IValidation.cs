namespace MicrolisR.Validation;

public interface IValidation
{
    internal void Validate<TObject>(TObject value)
        where TObject : IValidatable;
}

public interface IValidation<in TObject> : IValidation
    where TObject : IValidatable
{

    void IValidation.Validate<T>(T value)
    {
        if (value is TObject validatable)
        {
            Validate(validatable);
        }
    }
    
    public void Validate(TObject value);
}