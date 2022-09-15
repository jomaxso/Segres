using MicrolisR.Validation;

namespace MicrolisR;


/// <summary>
/// Marker interface to represent a request with a response.
/// </summary>
/// <typeparam name="T">The response type</typeparam>
public interface IQueryRequest<T> : IValidatable
{
}