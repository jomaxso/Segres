using MicrolisR.Validation;

namespace MicrolisR.Abstractions;


public interface IRequest<T> : IValidatable
{
}

public interface IRequest : IRequest<Unit>
{
}
