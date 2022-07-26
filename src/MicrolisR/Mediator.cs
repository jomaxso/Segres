namespace MicrolisR;

public sealed class Mediator : IMediator
{
    private readonly ISender _sender;
    private readonly IValidator _validator;
    private readonly IMapper _mapper;

    public Mediator(Func<Type, object> serviceResolver)
    {
        _sender = (ISender)serviceResolver(typeof(ISender));
        _validator = (IValidator)serviceResolver(typeof(IValidator));
        _mapper = (IMapper)serviceResolver(typeof(IMapper));
    }
    
    public Mediator(ISender sender, IValidator validator, IMapper mapper)
    {
        _sender = sender;
        _validator = validator;
        _mapper = mapper;
    }

    public Task<TResponse> SendAsync<TResponse>(IRequestable<TResponse> request, CancellationToken cancellationToken = default)
        => _sender.SendAsync(request, cancellationToken);
    
    public Task SendAsync(IRequestable request, CancellationToken cancellationToken = default)
        => _sender.SendAsync(request, cancellationToken);
    
    public TResponse Map<TResponse>(IMappable<TResponse> request)
        => _mapper.Map(request);

    public void Validate(IValidatable value) 
        => _validator.Validate(value);
}