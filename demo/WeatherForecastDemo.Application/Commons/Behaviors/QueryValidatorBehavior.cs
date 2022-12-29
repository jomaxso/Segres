using FluentValidation;
using Segres;
using Segres.Abstractions;

namespace WeatherForecastDemo.Application.Commons.Behaviors;

public sealed class QueryValidatorBehavior<TRequest, TResult> : IRequestBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    private readonly IValidator<TRequest>? _validator;

    public QueryValidatorBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }
    

    public TResult Handle(RequestHandlerDelegate<TResult> next, TRequest request)
    {
        var validationResult = _validator?.Validate(request);

        if (validationResult?.IsValid is false) throw new ValidationException(validationResult.Errors);

        return next(request);
    }
}