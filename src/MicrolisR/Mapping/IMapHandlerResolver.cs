namespace MicrolisR;

public interface IMapHandlerResolver
{
    T? Resolve<T>(object handler, IMappable<T> value);
}