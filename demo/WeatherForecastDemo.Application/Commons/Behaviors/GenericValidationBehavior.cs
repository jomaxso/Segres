using FluentValidation;
using Segres;

namespace WeatherForecastDemo.Application.Commons.Behaviors;

public class GenericValidationBehavior<TRequest, TResponse> : IRequestBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public GenericValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async ValueTask<TResponse> HandleAsync(RequestDelegate<TResponse> next, TRequest request, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next(request, cancellationToken);
        
        
        var response = await next(request, cancellationToken);

        return response;
    }
}