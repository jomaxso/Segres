using Demo.Domain.PrintToConsole;
using MicrolisR;

namespace Demo.Endpoints.PrintToConsole;

public class PrintEndpoint : IRequestHandler<PrintRequest, bool>
{
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;

    public PrintEndpoint(IMapper mapper, IPublisher publisher)
    {
        this._mapper = mapper;
        _publisher = publisher;
    }

    [Endpoint(Http.GET, "print/{value:int}")]
    public Task<bool> HandleAsync(PrintRequest request, CancellationToken cancellationToken = default)
    {
        _publisher.PublishAsync(new PrintMessage(request.ToString()!), cancellationToken);
        
        return Task.FromResult(true);
    }
}