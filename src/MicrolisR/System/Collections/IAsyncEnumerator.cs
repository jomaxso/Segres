namespace System.Collections;

public interface IAsyncEnumerator : IAsyncDisposable
{
    ValueTask<bool> MoveNextAsync();
    ValueTask Current { get; }
}