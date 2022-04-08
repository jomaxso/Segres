namespace MicrolisR.Mapping;

public interface IHandler
{
    object? Handle(object value);
}