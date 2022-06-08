namespace System.Collections;

public interface IAsyncEnumerable
{
    IAsyncEnumerator GetAsyncEnumerator(CancellationToken cancellationToken = default);
}