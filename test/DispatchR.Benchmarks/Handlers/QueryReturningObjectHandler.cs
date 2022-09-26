using DispatchR.Benchmarks.Contracts;

namespace DispatchR.Benchmarks.Handlers;

public class QueryReturningObjectHandler : IQueryHandler<QueryReturningObject, object>
{
    public Task<object> HandleAsync(QueryReturningObject request, CancellationToken cancellationToken)
    {
        return Task.FromResult<object>(null);
    }
}