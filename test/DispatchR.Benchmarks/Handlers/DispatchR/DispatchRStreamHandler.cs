using System.Runtime.CompilerServices;
using DispatchR.Benchmarks.Contracts;

namespace DispatchR.Benchmarks.Handlers;

public class DispatchRStreamHandler : IStreamHandler<UserStream, int?>
{
    /// <inheritdoc />
    public async IAsyncEnumerable<int?> HandleAsync(UserStream stream, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        foreach (var item in BenchmarkService.ListOfNumbers)
        {
            yield return item;
        }
    }
}