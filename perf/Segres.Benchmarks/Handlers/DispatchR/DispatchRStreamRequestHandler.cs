using System.Runtime.CompilerServices;
using Segres;
using Segres.Abstractions;

namespace DispatchR.Benchmarks.Handlers.DispatchR;

public class DispatchRStreamRequestHandler : IStreamRequestHandler<UserStreamRequest, int?>
{
    /// <inheritdoc />
    public async IAsyncEnumerable<int?> HandleAsync(UserStreamRequest streamRequest, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        foreach (var item in BenchmarkService.ListOfNumbers) yield return item;
    }
}