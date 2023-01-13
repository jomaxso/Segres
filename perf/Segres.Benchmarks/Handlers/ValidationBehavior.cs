using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Segres;
using Segres.Handlers;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace DispatchR.Benchmarks.Handlers;

public sealed class ValidationBehaviorSegres<TRequest, TResponse> : IRequestBehavior<TRequest, TResponse> where TRequest : Segres.Contracts.IRequest<TResponse>
{
    public ValueTask<TResponse> HandleAsync(RequestDelegate<TResponse> next, TRequest request, CancellationToken cancellationToken)
    {
        return next(request, cancellationToken);
    }
}

public sealed class ValidationBehaviorMediator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : global::MediatR.IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, global::MediatR.RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return next();
    }
}