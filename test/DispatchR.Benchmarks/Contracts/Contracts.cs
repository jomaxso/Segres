using DispatchR.Contracts;
using MediatR;

namespace DispatchR.Benchmarks.Contracts;

public record GetUsers : IQuery<int>, IRequest<int>;

public record CreateUser : ICommand, IRequest;
public record UserStream : IStream<int?>, IStreamRequest<int?>;

public record CreateUserWithResult : ICommand<int>, IRequest<int>;

public record UserCreated : IMessage, INotification;