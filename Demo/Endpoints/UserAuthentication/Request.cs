using MicrolisR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Endpoints.Authentication.User;

public class GetUserAuthenticationRequest : IRequestable<UserAuthenticationResponse>
{
     public int Value { get; set; }
     public Guid Id { get; set; }
}

public record GetAllAuthenticationsRequest : IRequestable<List<UserAuthenticationResponse>>
{
}

public record class UserAuthenticationResponse
{
    public int Value { get; set; }
};