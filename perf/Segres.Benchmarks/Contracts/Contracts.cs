using Segres;

namespace DispatchR.Benchmarks;

public record GetUsers : IRequest<int>, MediatR.IRequest<int>;

public record CreateUser : IRequest, MediatR.IRequest;

public record UserStreamRequest : IStreamRequest<int?>, MediatR.IStreamRequest<int?>;

public record CreateUserWithResult(int Number) : IRequest<int>, MediatR.IRequest<int>;
public record CreateUserWithResultSync(int Number) : IRequest<int>;

public record UserCreated : Segres.IEvent, MediatR.INotification;