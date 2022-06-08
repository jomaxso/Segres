namespace System.Linq;

public static partial class AsyncEnumerable
{
    public static IAsyncEnumerable<T> Create<T>(Func<CancellationToken, IAsyncEnumerator<T>> getAsyncEnumerator)
    {
        if (getAsyncEnumerator == null)
            throw new ArgumentNullException(nameof(getAsyncEnumerator));

        return new AnonymousAsyncEnumerable<T>(getAsyncEnumerator);
    }

    private sealed class AnonymousAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly Func<CancellationToken, IAsyncEnumerator<T>> _getEnumerator;

        public AnonymousAsyncEnumerable(Func<CancellationToken, IAsyncEnumerator<T>> getEnumerator) =>
            _getEnumerator = getEnumerator;

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

            return _getEnumerator(cancellationToken);
        }
    }
}