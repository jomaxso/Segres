using MediatR;
using Segres;
using Segres.Tmp.Http;
using INotification = Segres.INotification;
using IRequest = Segres.IRequest;

namespace DispatchR.Benchmarks;

public record GetUsers : Segres.IRequest<int>, MediatR.IRequest<int>;

public record CreateUser : IRequest, MediatR.IRequest;

public record UserStreamRequest : Segres.IStreamRequest<int?>, MediatR.IStreamRequest<int?>;

public record CreateUserWithResult(int Number) : Segres.IRequest<int>, MediatR.IRequest<int>;

public record UserCreated : INotification, MediatR.INotification;

public record CreateUser2 : IHttpRequest<bool>
{
    public static string RoutePattern { get; }
    public static Http HttpMethod { get; } = Http.POST;
}