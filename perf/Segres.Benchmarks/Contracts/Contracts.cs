using Segres;
using INotification = Segres.INotification;

namespace DispatchR.Benchmarks;

public record GetUsers : Segres.IRequest<int>, MediatR.IRequest<int>;

public record CreateUser : IRequest, MediatR.IRequest;

public record UserStreamRequest : Segres.IStreamRequest<int?>, MediatR.IStreamRequest<int?>;

public record CreateUserWithResult(int Number) : Segres.IRequest<int>, MediatR.IRequest<int>;
public record CreateUserWithResultSync(int Number) : Segres.IRequest<int>;

public record UserCreated : INotification, MediatR.INotification;