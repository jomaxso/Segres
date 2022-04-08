namespace MicrolisR.Mapping;

public interface IAsyncHandler
{
    Task<object?> HandleAsync(object value);
}