namespace MicrolisR;

public interface IValidationHandler<in T>
    where T : IValidatable
{
    public void Validate(T value);
}