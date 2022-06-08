using System.Linq.Expressions;

namespace System.Linq;

internal abstract class AsyncQueryable
{
    internal abstract object? Enumerable { get; }
    internal abstract Expression Expression { get; }
}

internal class AsyncQueryable<T> : AsyncQueryable, IOrderedAsyncQueryable<T>, IAsyncQueryProvider
{
    private static readonly Type _elementType = typeof(T);
    private readonly Expression _expression;
    private IAsyncEnumerable<T>? _enumerable;

    public AsyncQueryable(Expression expression)
    {
        this._expression = expression;
    }

    public AsyncQueryable(IAsyncEnumerable<T> enumerable)
    {
        this._expression = Expression.Constant(this);
        this._enumerable = enumerable;
    }

    Type IAsyncQueryable.ElementType => _elementType;
    Expression IAsyncQueryable.Expression => _expression;
    IAsyncQueryProvider IAsyncQueryable.Provider => this;

    internal override object? Enumerable => _enumerable;
    internal override Expression Expression => _expression;

    IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        this._enumerable ??= GetAsyncEnumerator(this._expression);
        return _enumerable.GetAsyncEnumerator(cancellationToken);
    }

    IAsyncQueryable<TElement> IAsyncQueryProvider.CreateQuery<TElement>(Expression expression)
        => new AsyncQueryable<TElement>(expression);

    ValueTask<TResult> IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expression);

        if (typeof(ValueTask<TResult>).IsAssignableFrom(expression.Type))
            return new AsyncQueryableExecutor<TResult>(expression).ExecuteAsync(cancellationToken);

        throw new ArgumentException("The specified expression is not assignable to the result type.",
            nameof(expression));
    }

    public override string? ToString()
    {
        if (this._expression is not ConstantExpression ce || ce.Value != this)
            return _expression.ToString();

        return _enumerable?.ToString() ?? string.Empty;
    }

    private static IAsyncEnumerable<T> GetAsyncEnumerator(Expression expression)
    {
        var body = new AsyncEnumerableRewriter()
            .Visit(expression);
            
        var expressionLambda = Expression
            .Lambda<Func<IAsyncEnumerable<T>>>(body, null);
                
        return expressionLambda.Compile()
            .Invoke();
    }
}