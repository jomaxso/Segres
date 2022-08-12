namespace MicrolisR.Validation;

public interface IValidator
{
    public void Validate<T>(T value)
        where T : IValidatable;
}