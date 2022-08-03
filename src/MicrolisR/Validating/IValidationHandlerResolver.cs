namespace MicrolisR;

public interface IValidationHandlerResolver
{
    void Resolve(object handler, IValidatable value);
}