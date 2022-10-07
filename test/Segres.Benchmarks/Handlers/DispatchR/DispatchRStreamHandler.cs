using System.Runtime.CompilerServices;
using DispatchR.Benchmarks.Contracts;
using Segres;
using Segres.Handlers;

namespace DispatchR.Benchmarks.Handlers.DispatchR;

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