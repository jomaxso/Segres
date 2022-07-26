namespace MicrolisR;


public interface IRequestable<T> : IValidatable
{
}

public interface IRequestable : IRequestable<Unit>
{
}
