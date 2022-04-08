namespace MicrolisR.Mapping.Abstractions;

public interface IMapperHandler
{
    T? Handle<T>(object? value);
}