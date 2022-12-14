using FluentValidation;
using FluentValidation.Results;
using Segres;

namespace WeatherForecastDemo.Application.Commons.Behaviors;

public sealed class CommandValidatorBehavior<TRequest> : IRequestBehavior<TRequest, None>
    where TRequest : IRequest
{
    private readonly IEnumerable<IValidator<TRequest>>? _validators;

    public CommandValidatorBehavior(IEnumerable<IValidator<TRequest>>? validator = null)
    {
        _validators = validator;
    }

    public ValueTask<None> HandleAsync(RequestDelegate<None> next, TRequest request, CancellationToken cancellationToken)
    {
        if (_validators is null)
            return next(request, cancellationToken);

        var validationErrors = _validators
            .Select(x => x.Validate(request))
            .Where(x => x.IsValid is false)
            .SelectMany(x => x.Errors)
            .ToArray();
        
        if (validationErrors.Any())
            throw new ValidationException(validationErrors);
        
        return next(request, cancellationToken);
    }
}