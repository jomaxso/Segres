using Demo.Domain.PrintToConsole;
using MicrolisR;

namespace Demo.Endpoints.Authentication.User;

public class UserAuthenticationEndpoint : IRequestHandler<GetUserAuthenticationRequest, UserAuthenticationResponse>
{
    private readonly ILogger<UserAuthenticationEndpoint> _logger;
    private readonly IPublisher _publisher;

    public UserAuthenticationEndpoint(ILogger<UserAuthenticationEndpoint> logger, IPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }
    
    [Endpoint(Http.POST, "/")]
    public async Task<UserAuthenticationResponse> HandleAsync(GetUserAuthenticationRequest request, CancellationToken cancellationToken)
    {
        
        var response = new UserAuthenticationResponse()
        {
            Value = (int)request.Value
        };

        await _publisher.PublishAsync(new PrintMessage(request.ToString()!), cancellationToken);
        
        return response;
    }
}