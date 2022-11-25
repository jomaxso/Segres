using FluentValidation;
using Segres;

namespace WeatherForecastDemo.Application.Commons.Behaviors;

public sealed class QueryValidatorBehavior<TRequest, TResult> : IRequestBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    private readonly IValidator<TRequest>? _validator;

    public QueryValidatorBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public ValueTask<TResult> HandleAsync(RequestDelegate<TResult> next, TRequest request, CancellationToken cancellationToken)
    {
        var validationResult = _validator?.Validate(request);

        if (validationResult?.IsValid is false) throw new ValidationException(validationResult.Errors);

        return next(request, cancellationToken);
    }
}