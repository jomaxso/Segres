using System.Runtime.CompilerServices;
using DispatchR.Benchmarks.Contracts;
using MediatR;

namespace DispatchR.Benchmarks.Handlers.MediatR;

public class MediatRStreamHandler : IStreamRequestHandler<UserStream, int?>
{
    /// <inheritdoc />
    public async IAsyncEnumerable<int?> Handle(UserStream request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        foreach (var item in BenchmarkService.ListOfNumbers)
        {
            yield return item;
        }
    }
}