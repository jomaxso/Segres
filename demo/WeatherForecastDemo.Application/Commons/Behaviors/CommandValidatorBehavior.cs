using FluentValidation;
using Segres;

namespace WeatherForecastDemo.Application.Commons.Behaviors;

public sealed class CommandValidatorBehavior<TRequest> : IRequestBehavior<TRequest, None>
    where TRequest : IRequest
{
    private readonly IValidator<TRequest>? _validator;

    public CommandValidatorBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public ValueTask<None> HandleAsync(RequestDelegate<None> next, TRequest request, CancellationToken cancellationToken)
    {
        var validationResult = _validator?.Validate(request);

        if (validationResult?.IsValid is false) throw new ValidationException(validationResult.Errors);

        return next(request, cancellationToken);
    }
}