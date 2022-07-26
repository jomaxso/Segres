namespace MicrolisR;

public interface IMapHandler<in TRequest, out TResponse>
    where TRequest : IMappable<TResponse>
{
    TResponse Map(TRequest request);
}