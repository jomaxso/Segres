using MicrolisR;

namespace Demo.Endpoints.Authentication.User;

public class UserAuthenticationEndpoint : 
    IRequestHandler<GetUserAuthenticationRequest, UserAuthenticationResponse>
{
    private ILogger<UserAuthenticationEndpoint> _logger;

    public UserAuthenticationEndpoint(ILogger<UserAuthenticationEndpoint> logger)
    {
        _logger = logger;
    }
    
    [Endpoint(Http.GET, "/{value:int}", false)]
    public Task<UserAuthenticationResponse> HandleAsync(GetUserAuthenticationRequest request, CancellationToken cancellationToken)
    {
        var response = new UserAuthenticationResponse()
        {
            Value = request.Value
        };
        return Task.FromResult(response);
    }
}