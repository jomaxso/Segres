namespace MicrolisR.Api;

public interface IEntity<TId>
    where TId : struct
{
    TId Id { get; init; }
}
