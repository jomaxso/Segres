using System.Runtime.CompilerServices;
using Segres;
using Segres.Handlers;
using Segres.UnitTest.Stream.Objects;

namespace Segres.UnitTest.Stream.Handlers;

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