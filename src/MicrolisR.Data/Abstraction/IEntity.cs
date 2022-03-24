namespace MicrolisR.Data.Abstraction;

public interface IEntity<TId>
    where TId : struct
{
    TId Id { get; init; }
}
