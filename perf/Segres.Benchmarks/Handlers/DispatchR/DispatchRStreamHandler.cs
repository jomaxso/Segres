using System.Runtime.CompilerServices;
using Segres;

namespace DispatchR.Benchmarks.Handlers.DispatchR;

public class DispatchRStreamHandler : IStreamHandler<UserStreamRequest, int?>
{
    /// <inheritdoc />
    public async IAsyncEnumerable<int?> HandleAsync(UserStreamRequest streamRequest, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        foreach (var item in BenchmarkService.ListOfNumbers) yield return item;
    }
}