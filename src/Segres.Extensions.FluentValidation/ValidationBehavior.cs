using Microsoft.Extensions.DependencyInjection;
using Segres.Abstractions;

namespace Segres.Extensions.FluentValidation;

internal sealed class ValidationBehavior<TRequest, TResponse> : IAsyncRequestBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{

    public ValueTask<TResponse> HandleAsync(AsyncRequestHandlerDelegate<TResponse> next, TRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}