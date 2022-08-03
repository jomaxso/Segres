using MicrolisR;

namespace Demo.Endpoints.Authentication.User;

public class UserAuthenticationEndpoint : IRequestHandler<GetUserAuthenticationRequest, UserAuthenticationResponse>
{
    private readonly ILogger<UserAuthenticationEndpoint> _logger;

    public UserAuthenticationEndpoint(ILogger<UserAuthenticationEndpoint> logger)
    {
        _logger = logger;
    }
    
    [Endpoint(Http.POST, "/")]
    public Task<UserAuthenticationResponse> HandleAsync(GetUserAuthenticationRequest request, CancellationToken cancellationToken)
    {
        
        var response = new UserAuthenticationResponse()
        {
            Value = (int)request.Value
        };
        

        _logger.LogInformation(request.Id.ToString());
        return Task.FromResult(response);
    }
}