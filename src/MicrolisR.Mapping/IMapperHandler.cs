namespace MicrolisR.Mapping;

public interface IMapperHandler
{
    T? Handle<T>(object value) where T : class;
}