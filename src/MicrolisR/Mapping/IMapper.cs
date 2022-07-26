namespace MicrolisR;

public interface IMapper
{
    TResponse Map<TResponse>(IMappable<TResponse> request);
}