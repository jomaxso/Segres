using MicrolisR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Endpoints.Authentication.User;

public record GetUserAuthenticationRequest : IRequestable<UserAuthenticationResponse>
{
    [FromRoute] public int Value { get; set; }
    [FromBody] public Guid Guid { get; set; }
} 

public record GetAllAuthenticationsRequest : IRequestable<List<UserAuthenticationResponse>>
{
    
}

public record class UserAuthenticationResponse 
{
    public int Value { get; set; }
};
