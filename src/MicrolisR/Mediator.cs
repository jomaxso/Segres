namespace MicrolisR;

public sealed class Mediator : IMediator
{
    private readonly ISender _sender;
    private readonly IValidator _validator;

    public Mediator(Func<Type, object> serviceResolver)
    {
        _sender = (ISender) serviceResolver(typeof(ISender));
        _validator = (IValidator) serviceResolver(typeof(IValidator));
    }

    public Mediator(ISender sender, IValidator validator)
    {
        _sender = sender;
        _validator = validator;
    }

    public Task<TResponse> SendAsync<TResponse>(IRequestable<TResponse> request, CancellationToken cancellationToken = default)
        => _sender.SendAsync(request, cancellationToken);

    public async Task<TResponse> SendAsync<TResponse>(IRequestable<TResponse> request, bool validate, CancellationToken cancellationToken = default)
    {
        if (validate)
            _validator.Validate(request);

        var response = await _sender.SendAsync(request, cancellationToken);

        if (response is IValidatable validatable && validate)
            _validator.Validate(validatable);

        return response;
    }

    public Task SendAsync(IRequestable request, CancellationToken cancellationToken = default)
        => _sender.SendAsync(request, cancellationToken);


    public Task SendAsync(IRequestable request, bool validate, CancellationToken cancellationToken = default)
    {
        if (validate)
            _validator?.Validate(request);
        
        return _sender.SendAsync(request, cancellationToken);
    }
}