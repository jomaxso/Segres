using System.Runtime.CompilerServices;
using MediatR;

namespace DispatchR.Benchmarks.Handlers.MediatR;

public class MediatRStreamHandler : IStreamRequestHandler<UserStreamRequest, int?>
{
    /// <inheritdoc />
    public async IAsyncEnumerable<int?> Handle(UserStreamRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        foreach (var item in BenchmarkService.ListOfNumbers) yield return item;
    }
}