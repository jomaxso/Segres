using System.Runtime.CompilerServices;
using DispatchR.UnitTest.Stream.Objects;

namespace DispatchR.UnitTest.Stream.Handlers;

public class IntegerStreamHandler : IStreamHandler<IntegerStream, int>
{
    public async IAsyncEnumerable<int> HandleAsync(IntegerStream stream, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        for (var i = 0; i < 10; i++)
        {
            yield return i;
        }
    }
}