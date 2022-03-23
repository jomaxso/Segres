namespace MicrolisR.Data.Abstraction;

public interface IEntity<TId>
{
    TId Id { get; init; }
}
