using MicrolisR;
using PrintToConsole;

namespace Demo.Endpoints.PrintToConsole;

public class PrintEndpoint : IRequestHandler<PrintRequest>
{
    private readonly IMediator _mediator;
    
    public PrintEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Validate]
    [Endpoint(Http.GET, "/")]
    public async Task HandleAsync(PrintRequest request, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        var command = _mediator.Map(request)!;
    }
}