using MicrolisR;

namespace Demo.Endpoints.PrintToConsole;

public class PrintEndpoint : IRequestHandler<PrintRequest, PrintResult>
{
    private readonly IMapper _mapper;

    public PrintEndpoint(IMapper mapper)
    {
        this._mapper = mapper;
    }

    [Endpoint(Http.GET, "print/{value:int}")]
    public async Task<PrintResult> HandleAsync(PrintRequest request, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        var command = _mapper.Map(request)!;
        return _mapper.Map(command);
    }
}