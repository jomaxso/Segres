using MicrolisR;

namespace Demo.Endpoints.Authentication.User;

public class UserAuthenticationEndpoint : 
    IRequestHandler<GetUserAuthenticationRequest, UserAuthenticationResponse>
    // IRequestHandler<GetAllAuthenticationsRequest, List<UserAuthenticationResponse>>
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

    // [Endpoint(HttpMethod.GET, "/")]
    // public Task<List<UserAuthenticationResponse>> HandleAsync(GetAllAuthenticationsRequest request, CancellationToken cancellationToken)
    // {
    //     var response = new List<UserAuthenticationResponse>()
    //     {
    //         new UserAuthenticationResponse(),
    //         new UserAuthenticationResponse()
    //     };
    //
    //     return Task.FromResult(response);
    // }
}