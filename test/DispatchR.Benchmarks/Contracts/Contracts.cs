using DispatchR.Contracts;

namespace DispatchR.Benchmarks.Contracts;

public record QueryReturningObject : IQuery<object>;

public record Command : ICommand;

public record CommandReturningObject : ICommand<object>;