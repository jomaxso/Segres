namespace Segres;

public class ThePerson2 : IRequest
{
    public ThePerson2(int Age)
    {
        this.Age = Age;
    }

    public int Age { get; set; }
}

public class ThePerson : IRequest<int>
{
    public ThePerson(int Age)
    {
        this.Age = Age;
    }

    public int Age { get; set; }
}

public sealed class ThePersonExceptionFilter : IRequestFilter<ThePerson>
{
    public ValueTask<ThePerson> HandleAsync(ThePerson request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(request);
    }
}

public sealed class ThePersonHandler : IRequestHandler<ThePerson, int>
{
    [RequestFilter<ThePersonExceptionFilter, ThePerson>]
    public ValueTask<int> HandleAsync(ThePerson request, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Handle " + request.Age);
        return ValueTask.FromResult(request.Age);
    }
}

public sealed class ThePersonHandlerValidator<TRequest, TResponse> : IRequestBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public ValueTask<TResponse> HandleAsync(RequestDelegate<TResponse> next, TRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public sealed class Validator<TRequest, TResponse> : IRequestBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ThePersonValidator? _personValidator;

    public Validator(ThePersonValidator? personValidator = null)
    {
        _personValidator = personValidator;
    }
    
    public ValueTask<TResponse> HandleAsync(RequestDelegate<TResponse> next, TRequest request, CancellationToken cancellationToken)
    {
        if (_personValidator is not null && request is ThePerson person) _personValidator.Validate(person);

        return next(request, cancellationToken);
    }
}

public sealed class ThePersonValidator
{
    public void Validate(ThePerson request)
    {
        request.Age = request.Age + 1;
        Console.WriteLine("[Valid] " + request.Age);
    }
}

public sealed class ThePersonHandlerValidatorTwo : IRequestBehavior<ThePerson, int>
{
    public async ValueTask<int> HandleAsync(RequestDelegate<int> next, ThePerson request, CancellationToken cancellationToken)
    {
        request.Age = request.Age + 1;
        Console.WriteLine("[2] Before " + request.Age);

        var result = await next(request, cancellationToken);

        result = result + 1;
        Console.WriteLine("[2] After " + result);

        return result;
    }

    public ValueTask<None> HandleAsync(RequestDelegate<None> next, ThePerson2 request, CancellationToken cancellationToken)
    {
        request.Age = request.Age + 1;
        Console.WriteLine("[1] Before " + request.Age);

        return next(request, cancellationToken);
    }
}

public sealed class ThePersonHandlerValidatorThree : IRequestBehavior<ThePerson, int>
{
    public async ValueTask<int> HandleAsync(RequestDelegate<int> next, ThePerson request, CancellationToken cancellationToken)
    {
        request.Age = request.Age + 1;
        Console.WriteLine("[3] Before " + request.Age);

        var result = await next(request, cancellationToken);

        result = result + 1;
        Console.WriteLine("[3] After " + result);

        return result;
    }

    public ValueTask<None> HandleAsync(RequestDelegate<None> next, ThePerson2 request, CancellationToken cancellationToken)
    {
        request.Age = request.Age + 1;
        Console.WriteLine("[1] Before " + request.Age);

        return next(request, cancellationToken);
    }
}