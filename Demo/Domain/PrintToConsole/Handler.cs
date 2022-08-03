using MicrolisR;

namespace Demo.Domain.PrintToConsole;

public class Handler : IRequestHandler<PrintCommand, bool>
{
    private readonly IValidator _validator;
    private readonly IMapper _mapper;

    public Handler(IValidator validator, IMapper mapper)
    {
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<bool> HandleAsync(PrintCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        _validator.Validate(request);

      
        // Console.WriteLine(result);
        return true;
    }
}

